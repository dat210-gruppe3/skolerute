using System;
using skolerute.notifications;
using UserNotifications;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using skolerute.utils;


[assembly: Dependency(typeof(skolerute.iOS.Notification))]
namespace skolerute.iOS
{
	public class Notification : INotification
	{

		public async void SendCalendarNotification(string title, string description, DateTime triggerTime)
		{
			// create the notification
			var notification = new UILocalNotification();

			// set the fire date (the date time in which it will fire)
			notification.FireDate = NSDate.FromTimeIntervalSinceNow(5);
			//notification.FireDate = triggerTime.ToNSDate();

			// configure the alert
			notification.AlertAction = "View Alert";
			notification.AlertBody = "Your 10 second alert has fired!";

			// modify the badge
			//int currentNr = (int)(SettingsManager.GetPreference("badge"));
			//notification.ApplicationIconBadgeNumber = currentNr + 1;
			//await SettingsManager.SavePreferenceAsync("badge", currentNr + 1);

			nint badge = UIApplication.SharedApplication.ApplicationIconBadgeNumber + 1;
			notification.ApplicationIconBadgeNumber = badge;

			// set the sound to be the default sound
			notification.SoundName = UILocalNotification.DefaultSoundName;

			// schedule it
			UIApplication.SharedApplication.ScheduleLocalNotification(notification);
			Console.WriteLine("Scheduled...");
		}
	}
}
