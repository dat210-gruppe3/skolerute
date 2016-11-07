using System.Collections.Generic;
using SQLite.Net;
using Xamarin.Forms;
using skolerute.data;
using SQLiteNetExtensions.Extensions;

namespace skolerute.db
{
    class DatabaseManager : IDatabaseManager
    {
        private SQLiteConnection database;
        object locker = new object();

        public DatabaseManager()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            if(TableExists<School>(database) || TableExists<CalendarDay>(database))
            {
                CreateNewDatabase();
            }
        }

        public static bool TableExists<T>(SQLiteConnection connection)
        {
            string query = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = ?";
            SQLiteCommand cmd = connection.CreateCommand(query, typeof(T).Name);
            return (cmd.ExecuteScalar<string>() != null);
        }

        public void CreateNewDatabase()
        {
            lock(locker) {
                database.CreateTable<CalendarDay>();
                database.CreateTable<School>();
            }
        }

        public IEnumerable<School> GetSchools()
        {
            lock (locker)
            {
                return database.Table<School>();
            }
        }

        public IEnumerable<CalendarDay> GetCalendarDays()
        {
            lock (locker)
            {
                return database.Table<CalendarDay>();
            }
        }

        public School GetSchool(int id)
        {
            lock (locker)
            {
                return database.Get<School>(id);
            }
        }

        public CalendarDay GetCalendarDay(int id)
        {
            lock (locker)
            {
                return database.Get<CalendarDay>(id);
            }
        }

        public void InsertSingle(object obj)
        {
            lock (locker)
            {
                database.Insert(obj);
            }
        }

        public void UpdateSingle(object obj)
        {
            lock (locker)
            {
                database.InsertOrReplaceWithChildren(obj);
            }
        }

        public int InsertList<T>(List<T> objList)
        {
            lock (locker)
            {
                return database.InsertAll(objList);
            }
        }

        public int DeleteSchool(int id)
        {
            lock (locker)
            {
                return database.Delete<School>(id);
            }
        }

        public int DeleteCalendarDay(int id)
        {
            lock (locker)
            {
                return database.Delete<CalendarDay>(id);
            }
        }

        public void DropTables()
        {
            lock (locker)
            {
                if(database != null)
                {
                    database.DropTable<School>();
                    database.DropTable<CalendarDay>();
                }
            }
        }

        public void DeleteDatabase()
        {
            DependencyService.Get<ISQLite>().DeleteDatabase();
        }
    }
}
