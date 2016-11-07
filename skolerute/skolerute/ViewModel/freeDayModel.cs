using System.Collections.ObjectModel;

namespace skolerute
{
	public class FreeDayModel
	{
		public string Name { get; set; }
		public string Comment { get; set; }
		public FreeDayModel()
		{
		}
	}

	public class GroupedFreeDayModel : ObservableCollection<FreeDayModel>
	{
		public string LongName { get; set; }
		public string ShortName { get; set; }
	}
}
