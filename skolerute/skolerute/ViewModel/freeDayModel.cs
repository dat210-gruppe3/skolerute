using System.Collections.ObjectModel;
using System;

namespace skolerute
{
	public class FreeDayModel
	{
		public string Name { get; set; }
		public string Comment { get; set; }
        public string TextColor { get; set; }
        public FreeDayModel()
		{
            TextColor = "Black";
		}

        public DateTime GetEndDate()
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("nb-NO");
            if (Comment.Contains("-"))
            {
                string[] splittedDate = Comment.Split('-');
                return DateTime.Parse(splittedDate[1].Trim(), culture, System.Globalization.DateTimeStyles.AssumeLocal);
                //return Convert.ToDateTime(splittedDate[1].Trim());
            }
            return DateTime.Parse(Comment.Trim(), culture, System.Globalization.DateTimeStyles.AssumeLocal);
            //return Convert.ToDateTime(Comment.Trim());
        }
<<<<<<< HEAD
    
=======
>>>>>>> 6914e55e56f0364fed6addd599dd19e5cac24272

		public DateTime GetStartDate()
		{
			if (Comment.Contains("-"))
			{
				string[] splittedDate = Comment.Split('-');
				return Convert.ToDateTime(splittedDate[0].Trim());
			}
			return Convert.ToDateTime(Comment.Trim());
		}

		public TimeSpan GetDateInterval()
		{
			return GetEndDate().Subtract(GetStartDate());
		}

	}

	public class GroupedFreeDayModel : ObservableCollection<FreeDayModel>
	{
		public string LongName { get; set; }
		public string ShortName { get; set; }
        public string Date { get; set; }
	}
}