using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using skolerute.db;
using System.IO;
using SQLite.Net;

[assembly: Dependency (typeof (skolerute.Droid.db.SQLiteAndroid))]
namespace skolerute.Droid.db
{
    class SQLiteAndroid : ISQLite
    {
        public SQLiteAndroid() {}

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "skoleruteSQLite.db3";
            // Documents folder
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename); 

            // Create and return a new database connection.
            var connection = new SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), path);
            return connection;
        }
    }
}