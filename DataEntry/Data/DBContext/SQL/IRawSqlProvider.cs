using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dataentry.Data.DBContext.SQL
{
    public interface IRawSqlProvider
    {
        string GetAllUserListings { get; }

        DbParameter GetDbParameter(string parameterName, object value);
    }
}
