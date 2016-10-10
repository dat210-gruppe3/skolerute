using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using SQLite.Net;
using skolerute.db;
using SQLite.Net.Async;

[assembly: Dependency(typeof(skolerute.iOS.db.SQLiteIOS))]
namespace skolerute.iOS.db
{
    class SQLiteIOS : ISQLite
    {
		private SQLiteConnectionWithLock _conn;

        public SQLiteIOS()
        {

        }

        private static string GetDatabasePath()
        {
            var sqliteFilename = "skoleruteSQLite.db3";
            // Find documents and library paths for IOS
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            return Path.Combine(libraryPath, sqliteFilename);
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
            try
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

                _conn = null;
            }
            catch
            {
                throw;
            }
        }

        public SQLiteConnection GetConnection()
        {
            var dbPath = GetDatabasePath();

            // Create and return new SQLite connection
            return new SQLiteConnection(new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS(), dbPath);
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();

            var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();

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
