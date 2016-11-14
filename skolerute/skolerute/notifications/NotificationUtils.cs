using System;
using System.Collections.ObjectModel;
namespace skolerute
{
	public class NotificationUtils
	{
		public NotificationUtils()
		{
		}

		public static DateTime calculateDateOfNotification(ObservableCollection<GroupedFreeDayModel> freeDayGroups, int daysBeforeNotification)
		{
			foreach (var item in freeDayGroups)
			{
				
			}

			//DateTime notificationDate = nextFreeDay.AddDays(-daysBeforeNotification);
			// Add an universal (static) point of time 
			//notificationDate.AddHours(9);
			//return notificationDate;
			return DateTime.Now;
		}
	}
}
