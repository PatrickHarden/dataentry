using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Repository;
using dataentry.Services.Business.Listings;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static dataentry.Services.Business.Publishing.PublishActionType;

namespace dataentry.Services.Business.Publishing
{

    public class PublishingService : IPublishingService
    {
        private readonly IListingMapper _listingMapper;
        private readonly IEnumerable<IPublishingTarget> _publishingTargets;
        private readonly IDataEntryRepository _dataEntryRepository;
        private readonly ILogger<PublishingService> _logger;
        private readonly bool _previewFeatureFlag;

        public PublishingService(
            IDataEntryRepository dataEntryRepository,
            IListingMapper listingMapper,
            IEnumerable<IPublishingTarget> publishingTargets,
            ILogger<PublishingService> logger,
            IConfiguration configuration)
        {
            _dataEntryRepository = dataEntryRepository ??
                throw new ArgumentNullException(nameof(dataEntryRepository));
            _listingMapper = listingMapper ??
                throw new ArgumentNullException(nameof(listingMapper));
            _publishingTargets = publishingTargets ??
                throw new ArgumentNullException(nameof(publishingTargets));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _previewFeatureFlag = bool.Parse(configuration["FeatureFlags:PreviewFeatureFlag"] ??
                throw new ArgumentException("PreviewFeatureFlag is null"));
        }

        /// <summary>
        /// Used to publish listing to store api
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ListingViewModel> PublishListingAsync(int id, ClaimsPrincipal user)
        {
            var publishingOptions = new PublishingOptions(id, user);
            return await RunPublishingAsync(publishingOptions);
        }

        /// <summary>
        /// Used to unpublish listings from the store api
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ListingViewModel> UnPublishListingAsync(int id, ClaimsPrincipal user)
        {
            var publishingOptions = new PublishingOptions(id, user, PublishActionType.Unpublish);
            return await RunPublishingAsync(publishingOptions);
        }

        /// <summary>
        /// Used to preview listing
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ListingViewModel> PreviewListingAsync(int id, ClaimsPrincipal user)
        {
            var publishingOptions = new PublishingOptions(id, user, PublishActionType.PublishPreview);
            return await RunPublishingAsync(publishingOptions);
        }

        /// <summary>
        /// Used to remove preview listing
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ListingViewModel> UnPreviewListingAsync(int id, ClaimsPrincipal user)
        {
            var publishingOptions = new PublishingOptions(id, user, PublishActionType.UnpublishPreview);
            return await RunPublishingAsync(publishingOptions);
        }

        public async Task<ListingViewModel> RunPublishingAsync(PublishingOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (!_previewFeatureFlag && options.IsPreview) return null;
            if (options.ListingId == 0) throw new ArgumentOutOfRangeException(nameof(options.ListingId));

            Listing listing = await _dataEntryRepository.GetListingByID(options.ListingId, options.UserPrincipal);
            await listing.VerifyExternalID();

            using(_logger.BeginScope(new Dictionary<string, object>{
                ["PublishingService_listingId"] = listing.ID,
                ["PublishingService_externalId"] = listing.ExternalID,
                ["PublishingService_publishAction"] = options.PublishAction.ToAlias()
            }))
            {
                _logger.LogDebug("PublishingService: Begin processing listing.");

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                // Check if listing is in a valid state for publishing
                if (options.Validate)
                {
                    IsValidPublishingStateForUpdate(listing);
                }

                var exceptions = new List<Tuple<IPublishingTarget, Exception>>();

                IEnumerable<IPublishingTarget> requestedTargets;
                if (options.PublishingTargets != null)
                {
                    requestedTargets = options.PublishingTargets.Select(targetName =>
                            _publishingTargets.FirstOrDefault(target =>
                                target.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase)
                            )
                        )
                        .Where(target => target != null);
                }
                else
                {
                    requestedTargets = _publishingTargets;
                }

                Func<IPublishingTarget, Listing, Task> action = options.PublishAction
                switch
                {
                    Publish => (t, l) => t.Publish(l),
                    Unpublish => (t, l) => t.Unpublish(l),
                    PublishPreview => (t, l) => t.PublishPreview(l),
                    UnpublishPreview => (t, l) => t.UnpublishPreview(l),
                    _ => (t, l) => Task.CompletedTask
                };

                foreach (var task in requestedTargets.Select(t => new Tuple<Task, IPublishingTarget>(action(t, listing), t)).ToArray())
                {
                    try
                    {
                        await task.Item1;
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(new Tuple<IPublishingTarget, Exception>(task.Item2, ex));
                    }
                };

                bool stop = false;
                foreach (var ex in exceptions)
                {
                    stop = stop || ex.Item1.StopOnException;
                    _logger.LogError(ex.Item2, "Error during publish process for listing {listingID}", options.ListingId);
                }
                if (stop)
                {
                    throw new PublishingException(options);
                }

                if (options.UpdatePublishingState)
                {
                    var newState = options.PublishAction
                    switch
                    {
                        Publish => PublishingStateEnum.Publishing.ToString(),
                        Unpublish => PublishingStateEnum.Unpublishing.ToString(),
                        _ => null
                    };

                    if (newState != null)
                    {
                        var publishingState = listing.GetListingData<PublishingState>() ?? new PublishingState();

                        // Update property state to published
                        publishingState.Value = newState;
                        publishingState.DateUpdated = DateTimeOffset.Now;
                        publishingState.DateListed = publishingState.DateListed;

                        listing.SetListingData(publishingState);
                        await _dataEntryRepository.UpdateListing(listing, options.UserPrincipal);
                    }
                }

                stopWatch.Stop();

                _logger.LogInformation("Listing published. Process took {PublishingService_processDuration}",
                    stopWatch.Elapsed);

                return _listingMapper.Map(listing);
            }
        }

        private void IsValidPublishingStateForUpdate(Listing listing)
        {
            string publishingState = listing.GetListingData<PublishingState>().Value;

            bool IsValid = true;
            if (publishingState == null || listing.IsDeleted)
                return;
            if (publishingState.Equals(PublishingStateEnum.Publishing.ToString(), StringComparison.InvariantCultureIgnoreCase))
                IsValid = false;
            if (publishingState.Equals(PublishingStateEnum.Unpublishing.ToString(), StringComparison.InvariantCultureIgnoreCase))
                IsValid = false;

            if (!IsValid)
                throw new InvalidOperationException($"This listing is in {publishingState} state, please wait until the current operation is completed.");
        }
    }
}