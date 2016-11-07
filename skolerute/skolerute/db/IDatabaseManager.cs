using System.Collections.Generic;
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
        void InsertSingle(object obj);
        void UpdateSingle(object obj);
        int InsertList<T>(List<T> objList);
        int DeleteSchool(int id);
        int DeleteCalendarDay(int id);
        void DropTables();
        void DeleteDatabase();
    }
}
