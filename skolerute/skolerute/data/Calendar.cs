using System;
using System.Collections.Generic;
using System.Linq;

namespace skolerute.data
{
	public class Calendar
	{
		public static int dayOfWeek(DateTime dt)
		{
			int dow = 100;
			switch (dt.DayOfWeek)
			{
				case DayOfWeek.Monday:
					dow = 0;
					break;
				case DayOfWeek.Tuesday:
					dow = 1;
					break;
				case DayOfWeek.Wednesday:
					dow = 2;
					break;
				case DayOfWeek.Thursday:
					dow = 3;
					break;
				case DayOfWeek.Friday:
					dow = 4;
					break;
				case DayOfWeek.Saturday:
					dow = 5;
					break;
				case DayOfWeek.Sunday:
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
		public static List<int> displayCal(int year, int month)
		{
			List<int> calendarArr = new List<int>();

			//insert heading (week days)
			string[] days = { "M", "T", "O", "T", "F", "L", "S" };
			for (int i = 0; i < days.Length; i++)
			{
				//TODO: render "col-heading-objects" with value "i"
				//Console.Write(days[i] + "\t");
			}
			//TODO: jump down a line
			//Console.Write("\n");


			//insert days from prior month
			int priorMonth = month - 1;
			int priorYear = year;
			if (priorMonth == 0)
			{
				priorMonth = 12;
				priorYear = year - 1;
			}
			DateTime dt = new DateTime(year, month, 1);
			int dow = dayOfWeek(dt);
			int daysInPriorMonth = DateTime.DaysInMonth(priorYear, priorMonth);
			for (int i = daysInPriorMonth - (dow - 1); i <= daysInPriorMonth; i++)
			{
				//TODO: render calendar-day-object with value "i"
				//Console.Write(i + "\t");
				calendarArr.Add(i);
			}


			//insert days from current month
			int daysInMonth = DateTime.DaysInMonth(year, month);
			for (int i = 1; i <= daysInMonth; i++)
			{
				//TODO: render calendar-day-object with value "i"
				//Console.Write(i + "\t");
				calendarArr.Add(i);
				dt = new DateTime(year, month, i);
				if (dayOfWeek(dt) == 6)
				{
					//TODO: jump down a line
					//Console.Write("\n");
				}
			}


			//insert days from future month
			int daysLeft = 6 - dayOfWeek(dt);
			if (daysLeft > 0)
			{
				for (int i = 1; i <= daysLeft; i++)
				{
					//TODO: render calendar-day-object with value "i"
					//Console.Write(i + "\t");
					calendarArr.Add(i);
				}
			}
			return calendarArr;
		}

        public static bool[] GetFreeDays(School school, int month)
        {
            List<CalendarDay> daysInSelectedMonth = FindCurrentMonth(school, month);
            bool[] freeDays = new bool[daysInSelectedMonth.Count];

            for(int i = 0; i < daysInSelectedMonth.Count; i++)
            {
                freeDays[i] = daysInSelectedMonth.ElementAt<CalendarDay>(i).isFreeDay;
            }
            return freeDays;
        }

        private static List<CalendarDay> FindCurrentMonth(School school, int month)
        {
            List<CalendarDay> allDays = school.calendar;
            List<CalendarDay> selectedDays = new List<CalendarDay>();

            foreach (CalendarDay day in allDays)
            {
                if(day.date.Month == month)
                {
                    selectedDays.Add(day);
                }
            }
            return selectedDays;   
        }
	}
}
