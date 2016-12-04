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
            if (schools == null || schools.Count == 0 || schools.First() == null || schools.First().calendar == null) return null;
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
            List<GroupedFreeDayModel> grouped = new List<GroupedFreeDayModel>();
            DateTime today = DateTime.Now;

            foreach (School skole in skoler)
            {
                for (int i = 0; i < skole.calendar.Count; i++)
                {
                    //GroupedFreeDayModel FreeDayGroup = null;
                    DateTime startDate = skole.calendar[i].Date;
                    string dateInterval = "";
                    bool singleDayHoliday = false;
                    bool startSummerVacation = false;
                    bool endSummerVacation = false;
                    string holidayName = ToTitle(skole.calendar[i].Comment);

                    //Start of calendar is in the second half of the summer vacation
                    if (i == 0) {
                        endSummerVacation = true;
                        holidayName = "Siste dag i sommerferien";
                    }
                    //Ignore weekends
                    else if (skole.calendar[i].Date.DayOfWeek == System.DayOfWeek.Saturday || skole.calendar[i].Date.DayOfWeek == System.DayOfWeek.Sunday)
                    {
                        continue;
                    }
                    //Handle uncommented freedays
                    else if (skole.calendar[i].IsFreeDay && skole.calendar[i].Comment == "")
                    {
                        holidayName = "Fri";
                    }
                    //Handle commented freedays that is not part of a vacation
                    else if (skole.calendar[i].IsFreeDay && !(skole.calendar[i].Comment.Substring(Math.Max(0, skole.calendar[i].Comment.Length - 5)) == "ferie"))
                    {
                        singleDayHoliday = true;
                    }

                    if (skole.calendar[i].IsFreeDay)
                    {
                        GroupedFreeDayModel FreeDayGroup = new GroupedFreeDayModel() { LongName = holidayName.Trim(), ShortName = "" };
                        if (singleDayHoliday)
                        {
                            dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;
                            FreeDayGroup.Date = skole.calendar[i].Date;
                        } else
                        {
                            if (i + 1 < skole.calendar.Count)
                            {
                                while (skole.calendar[i + 1].IsFreeDay)
                                {
                                    i++;

                                    if (i >= skole.calendar.Count - 1)
                                    {
                                        FreeDayGroup.LongName = "Starten av sommerferien";
                                        startSummerVacation = true;
                                        break;
                                    }
                                }
                            }

                            DateTime endDate = skole.calendar[i].Date;

                            if (startSummerVacation)
                            {
                                dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;
                            } else if(endSummerVacation)
                            {
                                dateInterval = endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                            } else
                            {   
                                dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year
                                    + " - " + endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                            }

                        }

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
                            if (FreeDayGroup.LongName == group.LongName && FreeDayGroup.Date.DayOfYear == group.Date.DayOfYear)
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
            grouped = grouped.OrderBy(o => o[0].GetStartDate()).ToList();
            return new ObservableCollection<GroupedFreeDayModel>(grouped);
            //return grouped;
        }

        public static string ToTitle(string input)
        {
            if(input != "")
            {
                return input[0].ToString().ToUpper() + input.Remove(0, 1).ToLower();
            }
            return "";
        }

    }
}