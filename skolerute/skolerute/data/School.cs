using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolerute.data
{
	class School
	{
		public int ID { get; set; }
		public string name { get; set; }
		public List<CalendarDay> calendar { get; set; }

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
