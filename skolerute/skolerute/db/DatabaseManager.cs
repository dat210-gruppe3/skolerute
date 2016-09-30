﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using Xamarin.Forms;
using skolerute.data;
using System.IO;

namespace skolerute.db
{
    class DatabaseManager : IDatabaseManager
    {
        private SQLiteConnection database;
        object locker = new object();

        public DatabaseManager()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
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

        public int InsertSingle(object obj)
        {
            lock (locker)
            {
                return database.Insert(obj);
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

        public void DeleteDatabase()
        {
            database.DropTable<CalendarDay>();
            database.DropTable<School>();
            database.Close();
            database.Dispose();
        }
    }
}
