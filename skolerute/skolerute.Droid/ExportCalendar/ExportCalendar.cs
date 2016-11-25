using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net.Nsd;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Java.Util;
using Xamarin.Forms;
using skolerute.ExportCalendar;

[assembly: Dependency(typeof(skolerute.Droid.ExportCalendar.ExportCalendar))]
namespace skolerute.Droid.ExportCalendar
{
    public class ExportCalendar : IExportCalendar
    {
        private int calendarId;

        public void ExportToCalendar(ObservableCollection<GroupedFreeDayModel> groupedFreedays, MyCalendar chosenCalendar)
        {
            if (chosenCalendar != null)
            {
                calendarId = int.Parse(chosenCalendar.Id);
                RemoveFromCalendar();

                foreach (GroupedFreeDayModel group in groupedFreedays)
                {
                    foreach (FreeDayModel school in group)
                    {
                        ExportEvent(string.Format("{0} - Skolerute", school.Name), 
                            string.Format("{0} for {1}", group.LongName, school.Name),
                            "", school.GetStartDate(), school.GetEndDate());
                    }
                } 
            }
        }

        private void ExportEvent(string title, string description, string reminder, DateTime stardDate, DateTime endDate)
        {
            ContentValues eventValues = new ContentValues();

            eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, calendarId);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, title);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, description);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMs(stardDate.Year, stardDate.Month - 1,
                stardDate.Day, stardDate.Hour, stardDate.Minute));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMs(endDate.Year, endDate.Month - 1,
                endDate.Day, endDate.Hour, endDate.Minute));

            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");

            var uri = Forms.Context.ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);
            Console.WriteLine("Uri for new event: {0}", uri);
        }

        public List<MyCalendar> GetCalendarInfo()
        {
            var calendarsUri = CalendarContract.Calendars.ContentUri;

            string[] calendarsProjection =
            {
                CalendarContract.Calendars.InterfaceConsts.Id,
                CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
                CalendarContract.Calendars.InterfaceConsts.AccountName
            };

            var cursor = Forms.Context.ContentResolver.Query(calendarsUri, calendarsProjection, null, null, null);

            if (cursor.MoveToFirst())
            {
                List<MyCalendar> myCalendars = new List<MyCalendar>();
                int calendarId;
                string calendarName;
                string calendarAccount;
                int i = 0;

                int idCol = cursor.GetColumnIndex(calendarsProjection[0]);
                int nameCol = cursor.GetColumnIndex(calendarsProjection[1]);
                int accountCol = cursor.GetColumnIndex(calendarsProjection[2]);

                do
                {
                    calendarId = cursor.GetInt(idCol);
                    calendarName = cursor.GetString(nameCol);
                    calendarAccount = cursor.GetString(accountCol);

                    myCalendars.Add(new MyCalendar(calendarId.ToString(), calendarName, calendarAccount));
                    i++;
                } while (cursor.MoveToNext());
                cursor.Close();
                return myCalendars;
            }
            return null;
        }

        private void RemoveFromCalendar()
        {
            var calendarsUri = CalendarContract.Calendars.ContentUri;
            var eventsUri = CalendarContract.Events.ContentUri;

            var cursor = Forms.Context.ContentResolver.Query(eventsUri, null, null, null,
                null);

            if (cursor.MoveToFirst())
            {
                int currentId;
                string currentTitle;
                int i = 0;

                int idCol = cursor.GetColumnIndex(CalendarContract.Events.InterfaceConsts.Id);
                int titleCol = cursor.GetColumnIndex(CalendarContract.Events.InterfaceConsts.Title);
                do
                {
                    currentId = cursor.GetInt(idCol);
                    currentTitle = cursor.GetString(titleCol);

                    // HACK: To avoid deleting all events in a calendar we check for a string that will always be in the title
                    // for our calendar events
                    if (currentTitle.Contains("- Skolerute"))
                    {
                        Forms.Context.ContentResolver.Delete(ContentUris.WithAppendedId(eventsUri, currentId), null, null);
                    }
                } while (cursor.MoveToNext());
            }
        }

        private long GetDateTimeMs(int year, int month, int day, int hour, int minute)
        {
            Calendar calendar = Calendar.GetInstance(Java.Util.TimeZone.Default);

            calendar.Set(Calendar.DayOfMonth, day);
            calendar.Set(Calendar.HourOfDay, hour);
            calendar.Set(Calendar.Minute, minute);
            calendar.Set(Calendar.Month, month);
            calendar.Set(Calendar.Year, year);

            return calendar.TimeInMillis;
        }
    }
}