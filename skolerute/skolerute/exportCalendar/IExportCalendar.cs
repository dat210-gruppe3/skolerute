using System;
namespace skolerute
{
	public interface IExportCalendar
	{
		void ExportToCalendar(string title, string description, string reminder);
		void RemoveFromCalendar();
	}
}
