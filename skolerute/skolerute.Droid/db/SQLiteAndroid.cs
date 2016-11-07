using System;
using Xamarin.Forms;
using skolerute.db;
using System.IO;
using SQLite.Net;
using SQLite.Net.Async;

[assembly: Dependency (typeof (skolerute.Droid.db.SQLiteAndroid))]
namespace skolerute.Droid.db
{
    class SQLiteAndroid : ISQLite
    {
        private SQLiteConnectionWithLock _conn;

        public SQLiteAndroid()
        {

        }

        private static string GetDatabasePath()
        {
            const string sqliteFilename = "skoleruteSQLite.db3";
            // Documents folder
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(documentsPath, sqliteFilename);
        }

        public void CloseConnection()
        {
            if(_conn != null)
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
                if(_conn == null)
                {
                    _conn.Close();
                }
            }
            catch
            {

            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public SQLiteConnection GetConnection()
        {
            var dbPath = GetDatabasePath();

            // Create and return a new database connection.
            return  new SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), dbPath);
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();

            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();

            var connectionFactory = new Func<SQLiteConnectionWithLock>(
                () =>
                {
                    if(_conn == null)
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
