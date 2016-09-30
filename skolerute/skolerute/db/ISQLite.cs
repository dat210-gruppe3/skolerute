using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Async;

namespace skolerute.db
{
    public interface ISQLite
    {
        void CloseConnection();
        void DeleteDatabase();
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
    }
}
