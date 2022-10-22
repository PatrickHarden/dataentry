using System;
using dataentry.Extensions;

namespace dataentry.Services.Business.Publishing
{
    [Serializable]
    public class PublishingException : Exception
    {
        private PublishingOptions _options;

        public override string Message => $"Error while publishing listing ID: {_options.ListingId}, User: {_options.UserPrincipal.Identity.Name}, Action: {_options.PublishAction.ToAlias()}";

        public PublishingException(PublishingOptions options)
        {
            _options = options;
        }
    }
}
