using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Async;
using SQLite.Net;
using Xamarin.Forms;
using skolerute.data;

namespace skolerute.db
{
    public class DatabaseManagerAsync : IDatabaseManagerAsync
    {
        private SQLiteAsyncConnection database;

        public DatabaseManagerAsync()
        {
            database = DependencyService.Get<ISQLite>().GetAsyncConnection();
            CreateNewDatabase();
        }

        public void CreateNewDatabase()
        {
            database.CreateTableAsync<School>();
            database.CreateTableAsync<CalendarDay>();
        }

        public async Task<int> DeleteCalendarDay(int id)
        {
            return await database.DeleteAsync<CalendarDay>(id as object);
        }

        public async Task<int> DeleteSchool(int id)
        {
            return await database.DeleteAsync<School>(id as object);
        }

        public async Task<CalendarDay> GetCalendarDay(int id)
        {
            return await database.GetAsync<CalendarDay>(id as object);
        }

        public async Task<List<CalendarDay>> GetCalendarDays()
        {
            return await database.Table<CalendarDay>().ToListAsync();
        }

        public async Task<School> GetSchool(int id)
        {
            return await database.GetAsync<School>(id as object);
        }

        public async Task<List<School>> GetSchools()
        {
            return await database.Table<School>().ToListAsync();
        }

        public async Task<int> InsertList<T>(List<T> objList)
        {
            return await database.InsertAllAsync(objList);
        }

        public async Task<int> InsertSingle(object obj)
        {
            return await database.InsertAsync(obj);
        }
    }
}
