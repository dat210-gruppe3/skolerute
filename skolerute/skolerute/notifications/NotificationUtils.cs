using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
using skolerute.data;

namespace skolerute.notifications
{
	public class NotificationUtils
	{
		public NotificationUtils()
		{
		}

		public static void SendNotifications(List<School> favoriteSchools)
		{
			ObservableCollection<GroupedFreeDayModel> freeDayGroups = Calendar.AddSchoolToList(favoriteSchools);
			foreach (var item in freeDayGroups)
			{
				string text = "Det nærmer seg fri for ";
				text += item.Count == 1 ? " alle favorittskoler." : item.Count + " skoler.";

				DateTime startDate = DateTime.MaxValue;
				foreach (var school in item)
				{
					startDate = school.GetStartDate() < startDate ? school.GetStartDate() : startDate;
				}
				startDate = startDate.AddHours(12);
				if (startDate < DateTime.Now) continue;
				DateTime dateOfnotification = startDate.AddDays(-7);
				DependencyService.Get<INotification>().SendCalendarNotification("Skolerute", text, dateOfnotification);
			}
		}
	}
}
