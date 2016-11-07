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
