using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace skolerute.data
{
    public static class Calendar
    {
        public static DateTime GetFirstRelevantDateTime(DateTime selectedDate)
        {
            DateTime firstDateInMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            
            // Need this special case because the weeks start on Sunday in America 
            if (firstDateInMonth.DayOfWeek == DayOfWeek.Sunday) return firstDateInMonth.AddDays(-6);

            return firstDateInMonth.AddDays(0 - (double)(firstDateInMonth.DayOfWeek - 1));
        }

        public static List<List<CalendarDay>> GetAllRelevantCalendarDays(List<School> schools, DateTime selectedDate)
        {
            if(schools == null || schools.Count == 0 || schools.First() == null) return null;
            DateTime startDate = GetFirstRelevantDateTime(selectedDate);
            
            int startIndex = schools.First().calendar.FindIndex(day => day.Date.DayOfYear == startDate.DayOfYear);

            List<List<CalendarDay>> schoolsCalendarList = new List<List<CalendarDay>>();

            foreach (School school in schools)
            {
                schoolsCalendarList.Add(school.calendar.GetRange(startIndex, Constants.ShownCalendarDaysCount));
            }
            return schoolsCalendarList;
        }

        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo myCultureInfo = new CultureInfo("nb-NO");
            System.Globalization.Calendar myCalendar = myCultureInfo.Calendar;

            CalendarWeekRule myCalendarWeekRule = myCultureInfo.DateTimeFormat.CalendarWeekRule;
            System.DayOfWeek myFirstDayOfWeek = myCultureInfo.DateTimeFormat.FirstDayOfWeek;

            return myCalendar.GetWeekOfYear(date, myCalendarWeekRule, myFirstDayOfWeek);
        }

        public static string MonthToString(int currentMonthNumber)
        {
            string monthName = new CultureInfo("nb-NO").DateTimeFormat.MonthNames[currentMonthNumber - 1];
            monthName = monthName[0].ToString().ToUpper() + monthName.Remove(0, 1);
            return monthName;
        }

        public static ObservableCollection<GroupedFreeDayModel> AddSchoolToList(List<School> skoler)
        {
            GroupedFreeDayModel FreeDayGroup = null;
            ObservableCollection<GroupedFreeDayModel> grouped = new ObservableCollection<GroupedFreeDayModel>();
            string dateInterval = "";
            DateTime today = DateTime.Now;

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
                        //dateInterval = startDate.ToString("dd/MM/yy") + " - " + endDate.ToString("dd/MM/yy");
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
                            //dateInterval = startDate.ToString("dd/MM/yy");
                        }
                        else
                        {
                            FreeDayGroup.LongName = "Slutten av sommerferien";
                            dateInterval = endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                            //dateInterval = startDate.ToString("dd/MM/yy");
                        }
                    }
                    //Handle other types of freedays
                    else if (skole.calendar[i].IsFreeDay)
                    {
                        string currentComment = skole.calendar[i].Comment;
                        DateTime startDate = skole.calendar[i].Date;
                        dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;
                        //dateInterval = startDate.ToString("dd/MM/yy");
                        FreeDayGroup = new GroupedFreeDayModel() { LongName = skole.calendar[i].Comment, ShortName = "", Date = dateInterval };
                    }

                    if (FreeDayGroup != null)
                    {
                        //Check if group is already created and add to it if it is
                        bool foundGroup = false;
                        FreeDayModel model = new FreeDayModel() { Name = skole.name, Comment = dateInterval };
                        
                        // Turn text grey if the holiday done
                        if (model.GetEndDate() < today)
                        {
                            model.TextColor = "#808080";
                        }

                        foreach (GroupedFreeDayModel group in grouped)
                        {
                            if (FreeDayGroup.LongName == group.LongName && FreeDayGroup.Date == group.Date)
                            {
                                //group.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                                group.Add(model);
                                foundGroup = true;
                            }
                        }

                        //If a group was not found then create a new one
                        if (!foundGroup)
                        {
                            //FreeDayGroup.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                            FreeDayGroup.Add(model);
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

                FreeDayModel model = new FreeDayModel() { Name = "Alle valgte skoler", Comment = tempDate };

                // Turn text grey if the holiday done
                if (model.GetEndDate() < today)
                {
                    model.TextColor = "#808080";
                }

                grouped[i].Add(model);
            }
            return grouped;
        }

    }
}