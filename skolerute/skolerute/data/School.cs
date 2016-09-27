using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions;


namespace skolerute.data
{
	class School
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string name { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<CalendarDay> calendar { get; set; }
		public string calendarBlobbed { get; set; }

		public School(int ID, string name, List<CalendarDay> calendar)
		{
			this.ID = ID;
			this.name = name;
			this.calendar = calendar;
		}

		public School()
		{
			this.ID = 0;
			this.name = "";
			this.calendar = null;
		}
	}


}
