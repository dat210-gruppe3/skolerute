using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace skolerute.ExportCalendar
{
	public interface IExportCalendar
	{
		Task ExportToCalendar(ObservableCollection<GroupedFreeDayModel> groupedFreedays, MyCalendar chosenCalendar);
	    List<MyCalendar> GetCalendarInfo();
	}
}
