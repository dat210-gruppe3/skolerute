using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using SQLite.Net;
using skolerute.db;
using System.IO;

[assembly: Dependency(typeof(skolerute.UWP.db.SQLiteUWP))]
namespace skolerute.UWP.db
{
    class SQLiteUWP : ISQLite
    {
        public SQLiteUWP() {}

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "skoleruteSQLite.db3";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);

            var connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            return connection;
        }
    }
}