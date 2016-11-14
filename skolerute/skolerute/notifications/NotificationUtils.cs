using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
namespace skolerute.notifications
{
	public class NotificationUtils
	{
		public NotificationUtils()
		{
		}

		public static void SendNotifications(ObservableCollection<GroupedFreeDayModel> freeDayGroups, int daysBeforeNotification)
		{
			List<DateTime> startDateList = new List<DateTime>();
			foreach (var item in freeDayGroups)
			{
				//startDateList.Add(item[0].GetStartDate());
				string text = "Det nærmer seg fri for ";
				text += item.Count == 1 ? " alle favorittskoler." : item.Count + " skoler.";

				DateTime startDate = DateTime.MaxValue;
				foreach (var school in item)
				{
					startDate = school.GetStartDate() < startDate ? school.GetStartDate() : startDate;
				}
				startDate = startDate.AddHours(12);
				if (startDate < DateTime.Now) continue;
				DateTime dateOfnotification = startDate.AddDays(-daysBeforeNotification);
				DependencyService.Get<INotification>().SendCalendarNotification("Skolerute", text, dateOfnotification);
			}
		}
	}
}
