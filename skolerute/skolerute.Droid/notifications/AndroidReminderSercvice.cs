using System;
using Android.App;
using Android.Content;
using Android.OS;
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

            long millis = (long)(triggerTime - DateTime.Now).TotalMilliseconds;
            long futureInMillis = SystemClock.ElapsedRealtime() + millis;
            alarmManager.Set(AlarmType.ElapsedRealtime, futureInMillis, pendingIntent);
        }

		public void RemoveCalendarNotification()
		{
            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReciever));
            PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.CancelCurrent).Cancel();
        }
    }
}