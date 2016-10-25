using System;
using skolerute.notifications;
using UserNotifications;
using Xamarin.Forms.Platform.iOS;

namespace skolerute.iOS
{
	public class Notification //: INotification
	{
		public Notification()
		{
			

		}


		//public void SendCalendarNotification(string title, string description, DateTime triggerTime)
		//{
		//	//DateExtensions.ToNSDate
		//	var content = new UNMutableNotificationContent();
		//	content.Title = "Notification Title";
		//	content.Subtitle = "Notification Subtitle";
		//	content.Body = "This is the message body of the notification.";
		//	content.Badge = 1;

		//	var trigger = UNCalendarNotificationTrigger.CreateTrigger(triggerTime.ToNSDate()., false);

		//	var requestID = "sampleRequest";
		//	var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

		//	UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
		//	{
		//		if (err != null)
		//		{
		//			// Do something with error...
		//		}
		//	});

		}
	}

