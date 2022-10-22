using Dapper;
using dataentry.Publishing.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace dataentry.Publishing.Repository
{
    /// <summary>
    /// This class is used to connect to the data entry database to update listings publish state
    /// </summary>
    public class PublishingDataEntryRepository : IPublishingDataEntryRepository
    {
        private readonly string _postgresConnectionString;

        public PublishingDataEntryRepository(IConfiguration configuration)
        {
            _postgresConnectionString = configuration.GetConnectionString("PublishingPostgres") ?? throw new ArgumentNullException("Postgres Connection String is null");
        }

        /// <summary>
        /// Used to get listings from the dataentry database with publish state
        /// </summary>
        /// <param name="publishState"></param>
        /// <returns></returns>
        public Task<IEnumerable<PublishListing>> GetPublishListings(PublishState publishState)
        {
            return Execute<PublishListing>($"SELECT * FROM public.getlistingsbystate('{publishState.ToString()}')");
        }

        /// <summary>
        /// Used to set a listings publish state
        /// </summary>
        /// <param name="publishState"></param>
        /// <returns></returns>
        public Task UpdatePublishListingState(int listingId, PublishState publishState)
        {
            return Execute<PublishListing>($"CALL public.updatelistingtostate({listingId}, '{publishState.ToString()}')");
        }

        private async Task<IEnumerable<T>> Execute<T>(string sproc, object param = null)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_postgresConnectionString))
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<T>(sproc, param);
            }
        }
    }
}
