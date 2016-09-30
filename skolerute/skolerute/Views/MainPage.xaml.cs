using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace skolerute
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			string csvLine = "2016-08-01,Auglend skole,Nei,Nei,Ja,\n2016-08-02,Auglend skole,Nei,Nei,Ja,\n\n";
			//CSVParser.StringParser(csvLine);
			string url = "http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/32d52130-ce7c-4282-9d37-3c68c7cdba92/download/skolerute-2016-17.csv";
			skolerute.db.DatabaseManager database = new db.DatabaseManager();

			CSVParser parser = new CSVParser(url, database);
			parser.StringParser(csvLine);
			//Label label = this.FindByName<Label>("t") as Label;
			//label.Text = "VIIIIIIIII";
			InitializeComponent();
			string s = "";
			List<data.School> d = database.GetSchools().ToList();
			for (int i = 0; i < d.Count; i++) 
			{
				s += d.ElementAt(i).name;
			}
			Label label = new Label() { Text="HEI", VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), HorizontalOptions = LayoutOptions.CenterAndExpand };
			label.Parent(stack);
			label.BindingContext = new { text = s };

		}
	}
}
