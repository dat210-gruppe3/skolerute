using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace skolerute.data
{
    public class CalendarDay
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        
        public DateTime Date { get; set; }
        public bool IsFreeDay { get; set; }
        public bool IsNotWorkDay { get; set; }
        public bool IsSfoDay { get; set; }
        public string Comment { get; set; }

        [ForeignKey(typeof(School))]
        public int schoolID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public School School { get; set; }

		public CalendarDay(DateTime date, bool isFreeDay, bool isNotWorkDay, bool isSFODay, string comment)
		{
			this.Date = date;
			this.IsFreeDay = isFreeDay;
			this.IsNotWorkDay = isNotWorkDay;
			this.IsSfoDay = isSFODay;
			this.Comment = comment;
		}

        public CalendarDay()
        {
            this.Date = Convert.ToDateTime("2001-01-01");
            this.IsFreeDay = false;
            this.IsNotWorkDay = false;
            this.IsSfoDay = false;
            this.Comment = "";
        }
    } 
}
