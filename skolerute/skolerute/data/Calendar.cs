using System;
using System.Collections.Generic;
using System.Linq;

namespace skolerute.data
{
	public class Calendar
	{
		public static int DayOfWeek(DateTime dt)
		{
			int dow = 100;
			switch (dt.DayOfWeek)
			{
				case System.DayOfWeek.Monday:
					dow = 0;
					break;
				case System.DayOfWeek.Tuesday:
					dow = 1;
					break;
				case System.DayOfWeek.Wednesday:
					dow = 2;
					break;
				case System.DayOfWeek.Thursday:
					dow = 3;
					break;
				case System.DayOfWeek.Friday:
					dow = 4;
					break;
				case System.DayOfWeek.Saturday:
					dow = 5;
					break;
                case System.DayOfWeek.Sunday:
					dow = 6;
					break;
			}
			return dow;
		}

		/// <summary>
		/// Supposed to take in values for year and month, and give out whatever necessary to render the
		/// calender in the GUI. This functiion is to be called upon from the View-model in MVC
		/// </summary>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		public static List<int> GetCal(int year, int month)
        {
            //insert days from prior month
            List<int> allDaysToBeShown = GetPriorMonth(year, month);

            //insert days from current month
            allDaysToBeShown.AddRange(GetCurrentMonth(year, month));


            //insert days from future month
            allDaysToBeShown.AddRange(GetDaysInNextMonth(35 - allDaysToBeShown.Count));
            return allDaysToBeShown;
        }

        private static List<int> GetDaysInNextMonth(int daysLeftToDisplay)
        {
            List<int> calendarDaysList = new List<int>();
            if (daysLeftToDisplay > 0)
            {
                for (int i = 1; i <= daysLeftToDisplay; i++)
                {
                    calendarDaysList.Add(i);
                }
            }

            return calendarDaysList;
        }

        private static List<int> GetCurrentMonth(int year, int month)
        {
            List<int> calendarDaysList = new List<int>();
            DateTime dt = new DateTime(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= daysInMonth; i++)
            {
                calendarDaysList.Add(i);
                dt = new DateTime(year, month, i);
            }

            return calendarDaysList;
        }

        private static List<int> GetPriorMonth(int year, int month)
        {
            List<int> calendarDaysList = new List<int>();
            int priorMonth = month - 1;
            int priorYear = year;

            if (priorMonth == 0)
            {
                priorMonth = 12;
                priorYear = year - 1;
            }

            DateTime dt = new DateTime(year, month, 1);
            int dow = DayOfWeek(dt);
            int daysInPriorMonth = DateTime.DaysInMonth(priorYear, priorMonth);
            for (int i = daysInPriorMonth - (dow - 1); i <= daysInPriorMonth; i++)
            {
                calendarDaysList.Add(i);
            }

            return calendarDaysList;
        }

        public static List<bool> GetAllFreeDays(List<CalendarDay> calendarDays, int year, int month)
        {
            List<CalendarDay> relevantDays = GetRelevantDays(calendarDays, year, month);
            List<bool> freeDays = GetFreeDaysInMonth(year, month - 1);

            return null;
        }

        private static List<CalendarDay> GetRelevantDays(List<CalendarDay> calendarDays, int year, int month)
        {
            for(int i = month - 1; i <= month + 1; i++)
            {
                DateTime current = new DateTime(year, i, 1);
                foreach (CalendarDay day in calendarDays)
                {
                    if (true)
                    {

                    }
                }
            }
            return null;
        }

        private static List<bool> GetFreeDaysInMonth(int year, int month)
        {
            return null;
        }
	}
}

