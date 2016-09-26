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

[assembly: Dependency(typeof(skolerute.WinPhone.db.SQLiteWinP))]
namespace skolerute.WinPhone.db
{
    class SQLiteWinP : ISQLite
    {
        public SQLiteWinP() {}

        public SQLiteConnection GetConnection()
        {
            string sqliteFilename = "skoleruteSQLite.db3";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);

            SQLiteConnection connection = new SQLiteConnection(path);
            return connection;
        }
    }
}
