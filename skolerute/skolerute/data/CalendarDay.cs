using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolerute.data
{
    class CalendarDay
    {
        public int ID { get; set; }
        public DateTime date { get; set; }
        public bool isFreeDay { get; set; }
        public bool isNotWorkDay { get; set; }
        public bool isSFODay { get; set; }
        public string comment { get; set; }

		public CalendarDay(int ID, DateTime date, bool isFreeDay, bool isNotWorkDay, bool isSFODay, string comment)
		{
			this.ID = ID;
			this.date = date;
			this.isFreeDay = isFreeDay;
			this.isNotWorkDay = isNotWorkDay;
			this.isSFODay = isSFODay;
			this.comment = comment;
		}
			
		public CalendarDay()
		{
			this.ID = 0;
			this.date = Convert.ToDateTime("2001-01-01");
			this.isFreeDay = false;
			this.isNotWorkDay = false;
			this.isSFODay = false;
			this.comment = "";
		}
    } 
}
