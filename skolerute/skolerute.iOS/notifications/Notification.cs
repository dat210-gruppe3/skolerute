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
		//https://github.com/xamarin/ios-samples/blob/master/LocalNotifications/Notifications/AppDelegate.cs
		//https://developer.xamarin.com/guides/ios/application_fundamentals/notifications/local_notifications_in_ios_walkthrough/

		public /*async*/ void SendCalendarNotification(string title, string description, DateTime triggerTime)
		{
			// create the notification
			var notification = new UILocalNotification();

			// set the fire date (triggerTime)
			//notification.FireDate = NSDate.FromTimeIntervalSinceNow(5);
			notification.FireDate = triggerTime.ToUniversalTime().ToNSDate();

			// configure the alert
			notification.AlertAction = "View Alert";
			notification.AlertBody = description;

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
