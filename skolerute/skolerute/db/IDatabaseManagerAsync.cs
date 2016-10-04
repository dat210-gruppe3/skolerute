using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skolerute.data;
using SQLite.Net.Async;

namespace skolerute.db
{
    interface IDatabaseManagerAsync
    {
        void CreateNewDatabase();
        Task<List<School>> GetSchools();
        Task<List<CalendarDay>> GetCalendarDays();
        Task<School> GetSchool(int id);
        Task<CalendarDay> GetCalendarDay(int id);
        Task<int> InsertSingle(object obj);
        Task<int> InsertList<T>(List<T> objList);
        Task<int> DeleteSchool(int id);
        Task<int> DeleteCalendarDay(int id);
    }
}
