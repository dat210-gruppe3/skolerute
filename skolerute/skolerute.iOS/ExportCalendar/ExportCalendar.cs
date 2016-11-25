using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AVFoundation;
using EventKit;
using EventKitUI;
using Foundation;
using skolerute.ExportCalendar;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(skolerute.iOS.ExportCalendar.ExportCalendar))]

namespace skolerute.iOS.ExportCalendar
{
    public class ExportCalendar : IExportCalendar
    {
        private string calendarId;
        private DateTime startDate;

        public void ExportToCalendar(ObservableCollection<GroupedFreeDayModel> groupedFreedays,
            MyCalendar chosenCalendar)
        {
            App.Current.EventStore.RequestAccess(EKEntityType.Event, (bool granted, NSError error) =>
            {
                if (granted)
                {
                    chosenCalendar = AskWhichCalendarToUse();

                    if (chosenCalendar != null)
                    {
                        InsertIntoChosenCalendar(groupedFreedays, chosenCalendar);
                    }
                }
                else
                {
                    new UIAlertView("Tilgang nektet",
                        "Brukeren har ikke gitt tilgang til kalenderen, dette kan endres i instillinger",
                        null, "Ok").Show();
                }
            });
        }

        private MyCalendar AskWhichCalendarToUse()
        {
            List<MyCalendar> myCalendars = GetCalendarInfo();
            MyCalendar chosenCalendar = null;

            if (myCalendars == null || myCalendars.Count == 0)
            {
                UIAlertController ac = UIAlertController.Create("Eksporter kalender", "Du har ingen tilgjengelige kalendere i kalender appen din.",
                    UIAlertControllerStyle.Alert);
            }
            else if (myCalendars.Count == 1) // Do this to not bother the user with popup messages when it is unnecessary
            {
                chosenCalendar = myCalendars.First();
            }
            else
            {
                UIAlertController chooseCalendarUI = UIAlertController.Create("Eksporter kalender", null, UIAlertControllerStyle.ActionSheet);
                chooseCalendarUI.AddAction(UIAlertAction.Create("Avbryt", UIAlertActionStyle.Cancel, action => {}));

                foreach (MyCalendar calendar in myCalendars)
                {
                    chooseCalendarUI.AddAction(UIAlertAction.Create(calendar.Name + " - " + calendar.Accout, UIAlertActionStyle.Default,
                        action =>
                        {
                            chosenCalendar = calendar;
                        }));
                }
            }

            return chosenCalendar;
        }

        private void InsertIntoChosenCalendar(ObservableCollection<GroupedFreeDayModel> groupedFreedays, MyCalendar chosenCalendar)
        {
            calendarId = chosenCalendar.Id;
            startDate = groupedFreedays.First().First().GetStartDate();
            RemoveFromCalendar();

            foreach (GroupedFreeDayModel group in groupedFreedays)
            {
                foreach (FreeDayModel school in group)
                {
                    ExportEvent(string.Format("{0} - Skolerute", school.Name),
                        string.Format("{0} for {1}", @group.LongName, school.Name),
                        "", school.GetStartDate(), school.GetEndDate());
                }
            }
        }

        private void ExportEvent(string title, string description, string reminder, DateTime stardDate, DateTime endDate)
        {
            EKEvent newEvent = EKEvent.FromStore(App.Current.EventStore);
            newEvent.StartDate = stardDate.ToNSDate();
            newEvent.EndDate = endDate.ToNSDate();
            newEvent.Title = title;
            newEvent.Notes = description;
            newEvent.Calendar = App.Current.EventStore.GetCalendar(calendarId);

            NSError error;
            App.Current.EventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out error);

            if (error != null)
            {
                Console.WriteLine(error.LocalizedFailureReason);
            }
            else
            {
                Console.WriteLine("New event created, ID: " + newEvent.CalendarItemIdentifier);
            }
        }

        private void RemoveFromCalendar()
        {
            // HACK: Since PredicateForEvents won't accept a single calendar, we have to make an array of calendars.
            // this is done for efficiency, since we only want to remove from one calendar at a time. Now it won't
            // search through multiple calendars, only the one the user selected.

            EKCalendar[] calendarsToQuery = new EKCalendar[1];
            calendarsToQuery[0] = App.Current.EventStore.GetCalendar(calendarId);

            NSPredicate query = App.Current.EventStore.PredicateForEvents(startDate.ToNSDate(),
                startDate.AddYears(1).ToNSDate(),
                calendarsToQuery);

            EKCalendarItem[] events = App.Current.EventStore.EventsMatching(query);
            foreach (EKEvent calendarEvent in events)
            {
                // HACK: To avoid deleting all events in a calendar we check for a string that will always be in the title
                // for our calendar events
                if (calendarEvent.Title.Contains("- Skolerute"))
                {
                    NSError error;
                    App.Current.EventStore.RemoveEvent(calendarEvent, EKSpan.ThisEvent, true, out error);

                    if (error != null)
                    {
                        Console.WriteLine(error.LocalizedFailureReason);
                    }
                    else
                    {
                        Console.WriteLine("Removed event, ID: " + calendarEvent.CalendarItemIdentifier);
                    }
                }
            }
        }

        public List<MyCalendar> GetCalendarInfo()
        {
            List<MyCalendar> myCalendars = new List<MyCalendar>();
            EKCalendar[] calendars = App.Current.EventStore.Calendars;

            foreach (EKCalendar calendar in calendars)
            {
                myCalendars.Add(new MyCalendar(calendar.CalendarIdentifier, calendar.Title, calendar.Type.ToString()));
            }

            return myCalendars;
        }
    }
}