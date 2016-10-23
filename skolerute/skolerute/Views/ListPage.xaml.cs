using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace skolerute.Views
{
	public partial class ListPage : ContentPage
	{
		public ListPage()
		{
			InitializeComponent();
			var layout = new StackLayout();
			this.Content = layout;
			var title = new Label { Text = "Neste fridag", FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand };
			layout.Children.Add(title);

			//Fetch favorite schools
			List<int> schoolIDs = new List<int>();
			MessagingCenter.Subscribe<StartUpPage, int>(this, "choosenSch", (sender, args) =>
			{
				schoolIDs.Add(args);
				//SchoolPicker.Items.Add(args.ToString());
			});


			DateTime[] dager = new DateTime[4] { new DateTime(2017,01,02), new DateTime(2017,04,11), new DateTime(2017,05,15), new DateTime(2017,05,08) };
			string[] skoler = new string[3] {"Gosen skole", "Tjennsvoll skole", "Austvoll skole"};
			List<object> liste = new List<object>();
			for (int i = 0; i < dager.Length; i++) {
				liste.Add(dager[i]);
				for (int j = 0; j < skoler.Length; j++) { 
					liste.Add(skoler[j]);
				}
			}
			ListView listview = new ListView { ItemsSource= liste, /*BackgroundColor = Color.Blue*/ };
			layout.Children.Add(listview);
		}


		//private static List<object> CreateListView(List<int> schoolIDs)
		//{
		//	DateTime now = DateTime.Now;
		//	int[] schIDsArray = schoolIDs.ToArray();
		//	data.School[] schools = new data.School[schIDsArray.Length];
		//	db.DatabaseManager db = new db.DatabaseManager();
		//	DateTime counter = now;
		//	List<object> listToReturn = new List<object>();

		//	for (int i = 0; i < schIDsArray.Length; i++)
		//	{
		//		schools[i] = db.GetSchool(schIDsArray[i]);
		//	}

		//	List<data.CalendarDay> schoolCal = schools[0].calendar;
		//	List<data.CalendarDay> freedays = data.Calendar.GetRelevantFreeDays(schoolCal, now);


		//	if (counter == freedays[1].date)
		//	{
		//		listToReturn.Add(counter);
		//		listToReturn.Add(counter);
		//	}


			
		//	return listToReturn;
		//}
	}
}
