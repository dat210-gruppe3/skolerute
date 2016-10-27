using System;
using skolerute.notifications;
using UserNotifications;
using Xamarin.Forms.Platform.iOS;
using Foundation;

namespace skolerute.iOS
{
	public class Notification //: INotification
	{
		public Notification()
		{


		}


		public void SendCalendarNotification(string title, string description, DateTime triggerTime)
		{
			NSDate nsdate = DateExtensions.ToNSDate(triggerTime);
			NSDateComponents NSTriggerTime = new NSDateComponents();
			NSTriggerTime.SetValueForComponent(triggerTime.Day, NSCalendarUnit.Day);
			NSTriggerTime.SetValueForComponent(triggerTime.Month, NSCalendarUnit.Month);
			NSTriggerTime.SetValueForComponent(triggerTime.Year, NSCalendarUnit.Year);
			NSTriggerTime.SetValueForComponent(triggerTime.Hour, NSCalendarUnit.Hour);
			NSTriggerTime.SetValueForComponent(triggerTime.Minute, NSCalendarUnit.Minute);
			NSTriggerTime.SetValueForComponent(triggerTime.Second, NSCalendarUnit.Second);

			//DateExtensions.ToNSDate
			var content = new UNMutableNotificationContent();
			content.Title = title;
			content.Subtitle = description;
			content.Body = "This is the message body of the notification.";
			content.Badge = 1;

			var trigger = UNCalendarNotificationTrigger.CreateTrigger(NSTriggerTime, false);

			var requestID = "sampleRequest";
			var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

			UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
			{
				if (err != null)
				{
					// Do something with error...
				}
			});

		}
	}
}
