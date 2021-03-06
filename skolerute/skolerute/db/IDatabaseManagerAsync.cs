using System.Collections.Generic;
using System.Threading.Tasks;
using skolerute.data;
using SQLite.Net.Async;

namespace skolerute.db
{
    interface IDatabaseManagerAsync
    {
        SQLiteAsyncConnection GetConnection();
        void CreateNewDatabase();
        Task<List<School>> GetSchools();
        Task<List<CalendarDay>> GetCalendarDays();
        Task<School> GetSchool(int id);
        Task<CalendarDay> GetCalendarDay(int id);
		Task<List<CalendarDay>> GetOnlyCalendar(int schoolID);
        Task InsertSingle(object obj);
        Task UpdateSingle(object obj);
        Task InsertList<T>(List<T> objList);
        Task InsertSchools(List<School> schools);
        Task<int> DeleteSchool(int id);
        Task<int> DeleteCalendarDay(int id);
    }
}
