using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Attributes;

namespace skolerute.data
{
    public class CalendarDay
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        
        public DateTime date { get; set; }
        public bool isFreeDay { get; set; }
        public bool isNotWorkDay { get; set; }
        public bool isSFODay { get; set; }
        public string comment { get; set; }

        [ForeignKey(typeof(School))]
        public int schoolID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public School school { get; set; }

		public CalendarDay(DateTime date, bool isFreeDay, bool isNotWorkDay, bool isSFODay, string comment)
		{
			this.date = date;
			this.isFreeDay = isFreeDay;
			this.isNotWorkDay = isNotWorkDay;
			this.isSFODay = isSFODay;
			this.comment = comment;
		}
			
		public CalendarDay()
		{
			this.date = Convert.ToDateTime("2001-01-01");
			this.isFreeDay = false;
			this.isNotWorkDay = false;
			this.isSFODay = false;
			this.comment = "";
		}
    } 
}
