using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace skolerute.data
{
    public static class Calendar
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
        /// calender in the GUI.
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
            allDaysToBeShown.AddRange(GetDaysInNextMonth(Constants.ShownCalendarDaysCount - allDaysToBeShown.Count));
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

            for (int i = 0; i < calendarDays.Count; i++)
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
                        lastMonth = new DateTime(dt.Year - 1, 12, calendar.ElementAt(0));
                    else
                        lastMonth = new DateTime(dt.Year, dt.Month - 1, calendar.ElementAt(0));
                }

                hasGottenToCorrectDay = currentDay.Date.CompareTo(lastMonth) >= 0;

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



        public static ObservableCollection<GroupedFreeDayModel> AddSchoolToList(List<School> skoler)
        {
            GroupedFreeDayModel FreeDayGroup = null;
            ObservableCollection<GroupedFreeDayModel> grouped = new ObservableCollection<GroupedFreeDayModel>();
            string dateInterval = "";

            foreach (School skole in skoler)
            {
                for (int i = 0; i < skole.calendar.Count; i++)
                {
                    CalendarDay currentDay = skole.calendar[i];
                    FreeDayGroup = null;
                    //Ignore weekends
                    if (skole.calendar[i].Date.DayOfWeek == System.DayOfWeek.Saturday || skole.calendar[i].Date.DayOfWeek == System.DayOfWeek.Sunday)
                    {
                        i++;
                    }
                    //Handle vacations
                    else if (currentDay.Comment.Substring(Math.Max(0, currentDay.Comment.Length - 5)) == "ferie")
                    {
                        FreeDayGroup = new GroupedFreeDayModel() { LongName = currentDay.Comment, ShortName = "" };
                        DateTime startDate = skole.calendar[i].Date;

                        if (i + 1 < skole.calendar.Count)
                        {
                            while (skole.calendar[i].IsFreeDay)
                            {
                                i++;

                                if (i >= skole.calendar.Count - 1)
                                {
                                    break;
                                }
                            }
                        }

                        DateTime endDate = skole.calendar[i].Date;
                        dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year
                            + " - " + endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                    }
                    //Handle summer vacation
                    else if (skole.calendar[i].IsFreeDay && skole.calendar[i].Comment == "")
                    {
                        FreeDayGroup = new GroupedFreeDayModel() { LongName = "Sommerferie", ShortName = "" };
                        DateTime startDate = skole.calendar[i].Date;

                        if (i + 1 < skole.calendar.Count)
                        {
                            while (skole.calendar[i].IsFreeDay)
                            {
                                i++;

                                if (i >= skole.calendar.Count - 1)
                                {
                                    break;
                                }
                            }
                        }

                        DateTime endDate = skole.calendar[i].Date;
                        //Check if end of dataset is reached
                        if (i == skole.calendar.Count - 1)
                        {
                            FreeDayGroup.LongName = "Starten av sommerferien";
                            dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;
                        }
                        else
                        {
                            FreeDayGroup.LongName = "Slutten av sommerferien";
                            dateInterval = endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                        }
                    }
                    //Handle other types of freedays
                    else if (skole.calendar[i].IsFreeDay)
                    {
                        string currentComment = skole.calendar[i].Comment;
                        DateTime startDate = skole.calendar[i].Date;
                        dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;
                        FreeDayGroup = new GroupedFreeDayModel() { LongName = skole.calendar[i].Comment, ShortName = "", Date = dateInterval };
                    }

                    if (FreeDayGroup != null)
                    {
                        //Check if group is already created and add to it if it is
                        bool foundGroup = false;
                        foreach (GroupedFreeDayModel group in grouped)
                        {
                            if (FreeDayGroup.LongName == group.LongName && FreeDayGroup.Date == group.Date)
                            {
                                group.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                                foundGroup = true;
                            }
                        }

                        //If a group was not found then create a new one
                        if (!foundGroup)
                        {
                            FreeDayGroup.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                            grouped.Add(FreeDayGroup);
                        }
                    }
                }
            }

            for (int i = 0; i < grouped.Count; i++)
            {
                //Check if current freeperiod is not the same for all schools
                if (grouped[i].Any(o => o.Comment != grouped[i][0].Comment))
                {
                    continue;
                }
                string tempDate = grouped[i][0].Comment;
                grouped[i].Clear();
                grouped[i].Add(new FreeDayModel() { Name = "Alle valgte skoler", Comment = tempDate });
            }
            return grouped;
        }

    }
}