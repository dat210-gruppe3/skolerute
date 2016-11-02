using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Async;
using SQLiteNetExtensionsAsync.Extensions;
using Xamarin.Forms;
using skolerute.data;

namespace skolerute.db
{
    public class DatabaseManagerAsync : IDatabaseManagerAsync
    {
        public SQLiteAsyncConnection connection { get; }

        public DatabaseManagerAsync()
        {
            connection = DependencyService.Get<ISQLite>().GetAsyncConnection();
        }

        public static async Task<bool> TableExists<T>(SQLiteAsyncConnection connection)
        {
            string query = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            return (await connection.ExecuteScalarAsync<string>(query, typeof(T).Name) != null);
        }

        public void CreateNewDatabase()
        {
            connection.CreateTableAsync<School>();
            connection.CreateTableAsync<CalendarDay>();
        }

        public async Task<int> DeleteCalendarDay(int id)
        {
            return await connection.DeleteAsync<CalendarDay>(id as object);
        }

        public async Task<int> DeleteSchool(int id)
        {
            return await connection.DeleteAsync<School>(id as object);
        }

        public async Task<CalendarDay> GetCalendarDay(int id)
        {
            return await connection.GetAsync<CalendarDay>(id as object);
        }

        public async Task<List<CalendarDay>> GetCalendarDays()
        {
            return await connection.Table<CalendarDay>().ToListAsync();
        }

		public async Task<List<CalendarDay>> GetOnlyCalendar(int schoolID)
		{
            return await connection.QueryAsync<CalendarDay>("SELECT * FROM CalendarDay WHERE schoolID=?", schoolID);
        }

        public async Task<School> GetSchool(int id)
        {
            return await connection.GetAsync<School>(id);
        }

        public async Task<List<School>> GetSchools()
        {
            return await connection.Table<School>().ToListAsync();
        }

        public async Task InsertList<T>(List<T> objList)
        {
            await connection.InsertAllAsync(objList);
        }

        public async Task InsertSchools(List<School> schools)
        {
            await connection.InsertAllAsync(schools);
        }

        public async Task InsertSingle(object obj)
        {
            await connection.InsertAsync(obj);
        }

        public async Task UpdateSingle(object obj)
        {
            await connection.InsertOrReplaceWithChildrenAsync(obj);
        }
    }
}

