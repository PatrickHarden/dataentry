using dataentry.Publishing.Models;
using dataentry.Publishing.Repository;
using dataentry.Publishing.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace dataentry.Publishing.Commands
{
    /// <summary>
    /// This class is used to update a publish listing
    /// </summary>
    public class UpdatePublishListingsState : ICommand
    {
        private readonly IPublishingDataEntryRepository _publishingDataEntryRepository;
        private readonly IStorePublishService _storePublishService;
        private readonly string _expirationTimeInMin;

        public UpdatePublishListingsState(
            IPublishingDataEntryRepository publishingDataEntryRepository,
            IStorePublishService storePublishService,
            ILogger log)
        {
            // DI
            _publishingDataEntryRepository = publishingDataEntryRepository ?? throw new ArgumentNullException(nameof(publishingDataEntryRepository));
            _storePublishService = storePublishService ?? throw new ArgumentNullException(nameof(storePublishService));

            // Environment Vars
            _expirationTimeInMin = Environment.GetEnvironmentVariable("ExpirationTimeInMin") ?? throw new ArgumentNullException("ExpirationTimeInMin is null");
        }

        /// <summary>
        /// Used to change the state of a listing from an inital state to a desired state
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="desiredState"></param>
        public async void Execute(PublishState initialState, PublishState desiredState, ILogger log)
        {
            // Get listings in the submitted state
            var listings = await _publishingDataEntryRepository.GetPublishListings(initialState);

            log.LogInformation($"{listings.Count()} listings found in state {initialState.ToString()}");

            // Check if each listings is in desired state
            foreach (var listing in listings)
            {
                var listingLogName = $"({listing.Id}) {listing.ExternalID}";
                log.LogInformation($"Processing: {listingLogName}");

                // Call search api to return the listing is in desired state
                var isInDesiredState =  await _storePublishService.IsListingInDesiredStateAsync(listing, desiredState, log);

                // Update listings state to published
                if (isInDesiredState)
                {
                    log.LogInformation($"Listing {listingLogName} was in desired state {desiredState.ToString()}");

                    await _publishingDataEntryRepository.UpdatePublishListingState(listing.Id, desiredState);
                }
                else
                {
                    log.LogInformation($"Listing {listingLogName} was not in desired state {desiredState.ToString()}");

                    // Backoff process after expiration
                    var backoffTime = int.Parse(_expirationTimeInMin);

                    if (listing.DateUpdated < DateTimeOffset.Now.AddMinutes(backoffTime * -1))
                    {
                        // If state doesn't match the desired state & the process already been running expiration time, release the lock and set to fail state
                        PublishState failState = desiredState == PublishState.Published ? PublishState.PublishFailed : PublishState.UnpublishFailed;
                        await _publishingDataEntryRepository.UpdatePublishListingState(listing.Id, failState);
                    }
                }
            }
        }
    }
}
