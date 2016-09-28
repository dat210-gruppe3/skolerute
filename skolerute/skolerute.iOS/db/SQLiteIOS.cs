using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using SQLite.Net;
using skolerute.db;

[assembly: Dependency(typeof(skolerute.iOS.db.SQLiteIOS))]
namespace skolerute.iOS.db
{
    class SQLiteIOS : ISQLite
    {
        public SQLiteIOS() {}

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "skoleruteSQLite.db3";
            // Find documents and library paths for IOS
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);

            // Create and return new SQLite connection
            var connection = new SQLiteConnection(new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS(), path);
            return connection; 
        }
    }
}
