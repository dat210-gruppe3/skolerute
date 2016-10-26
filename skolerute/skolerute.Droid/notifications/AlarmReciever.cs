using System;
using Android.Support.V4.App;
using Android.App;
using Android.Content;


// This will not function without the reminder service
[assembly: Xamarin.Forms.Dependency(typeof(skolerute.Droid.notifications.AndroidReminderSercvice))]
namespace skolerute.Droid.notifications
{
    [BroadcastReceiver]
    public class AlarmReciever : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string description = intent.GetStringExtra("description");
            string title = intent.GetStringExtra("title");

            var notIntent = new Intent(context, typeof(MainActivity));
            var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
            var manager = NotificationManagerCompat.From(context);

            var style = new NotificationCompat.BigTextStyle();
            style.BigText(description);

            var builder = new NotificationCompat.Builder(context)
                .SetContentIntent(contentIntent)
                .SetSmallIcon(Resource.Drawable.ic_pause_light) // TODO: Add launcher image here
                .SetContentTitle(title)
                .SetContentText(description)
                .SetStyle(style)
                .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
                .SetAutoCancel(true);

            var notification = builder.Build();
            manager.Notify(0, notification);
        }
    }
}