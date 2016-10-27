using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using skolerute.notifications;

namespace skolerute.Droid.notifications
{
    public class AndroidReminderSercvice : INotification 
    {

        public void SendCalendarNotification(string title, string description, DateTime triggerTime)
        {
            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReciever));
            alarmIntent.PutExtra("description", description);
            alarmIntent.PutExtra("title", title);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.CancelCurrent);
            AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

            // TODO: Set trigger time to the input trigger time, this is a demo which triggers after 5 seconds
            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 10 * 1000, pendingIntent);
        }
    }
}