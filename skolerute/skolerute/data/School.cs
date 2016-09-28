using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions;


namespace skolerute.data
{
	public class School
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string name { get; set; }

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<CalendarDay> calendar { get; set; }

		public School(string name, List<CalendarDay> calendar)
		{
			this.name = name;
			this.calendar = calendar;
		}

		public School()
		{
			this.name = "";
			this.calendar = null;
		}
	}


}