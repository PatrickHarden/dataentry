using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;

namespace dataentry.Data.DBContext.SQL
{
    public class RawPSQLProvider : IRawSqlProvider
    {
        private readonly Assembly _assembly = typeof(RawPSQLProvider).Assembly;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKeyFormat = typeof(RawPSQLProvider).FullName + ".{0}";
        public DbParameter GetDbParameter(string parameterName, object value) => new NpgsqlParameter(parameterName, value);
        public string GetAllUserListings => GetRawSql(nameof(GetAllUserListings));

        public RawPSQLProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        private string GetCacheKey(string key) => string.Format(_cacheKeyFormat, key);

        private string GetRawSql(string sqlName)
        {
            var key = GetCacheKey(sqlName);
            if (_cache.TryGetValue(key, out string result)) return result;

            using (var stream = _assembly.GetManifestResourceStream($"dataentry.Data.DBContext.SQL.PSQL.{sqlName}.sql"))
            using (var reader = new StreamReader(stream))
            {
                return _cache.Set(key, reader.ReadToEnd());
            }
        }
    }
}
