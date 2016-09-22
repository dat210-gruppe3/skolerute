using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using skolerute.data;

namespace skolerute.db
{
    class DatabaseManager
    {
        private SQLiteConnection database;
        object locker = new object();

        public DatabaseManager()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
        }

        public void CreateNewDatabase()
        {
            database.CreateTable<Calendar>();
            database.CreateTable<School>();
        }
        
        public IEnumerable<School> GetSchools()
        {
            lock (locker)
            {
                return database.Table<School>();
            }
        }

        public IEnumerable<Calendar> GetCalendars()
        {
            lock (locker)
            {
                return database.Table<Calendar>();
            }
        }

        public School GetSchool(int id)
        {
            lock (locker)
            {
                return database.Get<School>(id);
            }
        }

        public Calendar GetCalendar(int id)
        {
            lock (locker)
            {
                return database.Get<Calendar>(id);
            }
        }

        public int InsertSingle(object obj)
        {
            lock (locker)
            {
                return database.Insert(obj);
            }
        }

        public int InsertList(List<object> objList)
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

        public int DeleteCalendar(int id)
        {
            lock (locker)
            {
                return database.Delete<Calendar>(id);
            }
        }
    }
}
