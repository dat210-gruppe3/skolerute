using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;
using SQLite;
using skolerute.db;
using SQLite.Net;
using SQLite.Net.Async;

[assembly: Dependency(typeof(skolerute.WinPhone.db.SQLiteWinP))]
namespace skolerute.WinPhone.db
{
    class SQLiteWinP : ISQLite
    {
        private SQLiteConnectionWithLock _conn;

        public SQLiteWinP()
        {

        }

        private static string GetDatabasePath()
        {
            var sqliteFilename = "skoleruteSQLite.db3";
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
        }

        public void CloseConnection()
        {
            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
                _conn = null;

                // Connection is not disposed of until garbage collector cleans up
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void DeleteDatabase()
        {
            var path = GetDatabasePath();

            try
            {
                if (_conn == null)
                {
                    _conn.Close();
                }
            }
            catch
            {

            }

            StorageFile file = StorageFile.GetFileFromPathAsync(path).GetResults();
            if (file != null)
            {
                file.DeleteAsync().GetResults();
            }
        }

        public SQLiteConnection GetConnection()
        {
            var dbPath = GetDatabasePath();

            return new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), dbPath);
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();

            var platform = new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT();

            var connectionFactory = new Func<SQLiteConnectionWithLock>(
                () =>
                {
                    if (_conn == null)
                    {
                        _conn = new SQLiteConnectionWithLock(platform,
                            new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: true));
                    }
                    return _conn;
                });

            return new SQLiteAsyncConnection(connectionFactory);
        }
    }
}
