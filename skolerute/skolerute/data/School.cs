using System.Collections.Generic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;


namespace skolerute.data
{
	public class School
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string name { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string address { get; set; }
		public string website { get; set; }


		[OneToMany("schoolID", CascadeOperations = CascadeOperation.All)]
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