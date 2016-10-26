using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolerute.notifications
{
    public interface INotification
    {
        void SendCalendarNotification(string title, string description, DateTime triggerTime);
    }
}
