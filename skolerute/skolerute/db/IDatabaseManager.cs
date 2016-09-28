using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skolerute.data;

namespace skolerute.db
{
    public interface IDatabaseManager
    {
        void CreateNewDatabase();
        IEnumerable<School> GetSchools();
        IEnumerable<CalendarDay> GetCalendarDays();
        School GetSchool(int id);
        CalendarDay GetCalendarDay(int id);
        int InsertSingle(object obj);
        int InsertList<T>(List<T> objList);
        int DeleteSchool(int id);
        int DeleteCalendarDay(int id);
        void DeleteDatabase();
    }
}
