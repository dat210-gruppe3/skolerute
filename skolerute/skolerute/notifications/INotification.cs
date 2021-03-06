using System;

namespace skolerute.notifications
{
    public interface INotification
    {
        void SendCalendarNotification(string title, string description, DateTime triggerTime);
		void RemoveCalendarNotification(int notificationId);
    }
}
