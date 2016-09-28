using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using SQLite.Net;
using skolerute.db;

[assembly: Dependency(typeof(skolerute.Windows.db.SQLiteWin81))]
namespace skolerute.Windows.db
{
    class SQLiteWin81 : ISQLite
    {
        public SQLiteWin81() {}

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "skoleruteSQLite.db3";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);

            var connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT() ,path);
            return connection;
        }
    }
}
