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
		public static List<int> GetCal(DateTime dt)
        {
            //insert days from prior month
            List<int> allDaysToBeShown = GetPriorMonth(dt);

            //insert days from current month
            allDaysToBeShown.AddRange(GetCurrentMonth(dt));


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

        private static List<int> GetCurrentMonth(DateTime dt)
        {
            List<int> calendarDaysList = new List<int>();
            dt = new DateTime(dt.Year, dt.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(dt.Year, dt.Month);

            for (int i = 1; i <= daysInMonth; i++)
            {
                calendarDaysList.Add(i);
                dt = new DateTime(dt.Year, dt.Month, i);
            }

            return calendarDaysList;
        }

        private static List<int> GetPriorMonth(DateTime dt)
        {
            List<int> calendarDaysList = new List<int>();
            
           
            dt = new DateTime(dt.Year, dt.Month, 1);
            int dow = DayOfWeek(dt);
            dt = dt.AddMonths(-1);
            int daysInPriorMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
            for (int i = daysInPriorMonth - (dow - 1); i <= daysInPriorMonth; i++)
            {
                calendarDaysList.Add(i);
            }

            return calendarDaysList;
        }

        public static List<CalendarDay> GetRelevantFreeDays(List<CalendarDay> calendarDays, DateTime dt)
        {
            List<int> calendar = GetCal(dt); //All visible days
            List<CalendarDay> relevantDays = new List<CalendarDay>(); //The relevant calendar days in regards to shown days

            for(int i = 0; i < calendarDays.Count; i++)
            {
                CalendarDay currentDay = calendarDays.ElementAt(i);
                DateTime lastMonth = DateTime.Now;
                bool hasGottenToCorrectDay = false;

                if (calendar.ElementAt(0) == 1)
                {
                    lastMonth = new DateTime(dt.Year, dt.Month, calendar.ElementAt(0));
                }
                else
                {
                    if (dt.Month <= 1)
                        lastMonth = new DateTime(dt.Year-1, 12, calendar.ElementAt(0));
                    else
                        lastMonth = new DateTime(dt.Year, dt.Month - 1, calendar.ElementAt(0));
                }

                hasGottenToCorrectDay = currentDay.date.CompareTo(lastMonth) >= 0;

                if (hasGottenToCorrectDay)
                {
                    //Adds the CalendarDay objects corresponding to the shown months
                    return GetRelevantCalendarDays(calendarDays, calendar, relevantDays, i);
                }
            }
            
            return relevantDays;
        }

        private static List<CalendarDay> GetRelevantCalendarDays(List<CalendarDay> calendarDays, List<int> calendar, List<CalendarDay> relevantDays, int i)
        {
            for (int j = 0; j < calendar.Count; j++)
            {
                relevantDays.Add(calendarDays.ElementAt(i + j));
            }
            return relevantDays;
        }
    }
}

