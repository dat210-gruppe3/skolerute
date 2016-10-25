﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using skolerute.data;

namespace skolerute.Views
{
	public partial class ListPage : ContentPage
	{	//FØLGENDE LINJE ER DEL AV EKSEMPELKODE
		private ObservableCollection<GroupedVeggieModel> grouped { get; set; }

		public ListPage()
		{
			/*InitializeComponent();
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
			ListView listview = new ListView { ItemsSource= liste, };
			layout.Children.Add(listview);*/

			//_____________________________________________________________________________________
			//TESTER EKSEMPELKODE: https://github.com/xamarin/xamarin-forms-samples/blob/master/UserInterface/ListView/Grouping/groupingSampleListView/groupingSampleListView/Views/GroupedListXaml.xaml.cs

			InitializeComponent();

			ObservableCollection<School> favoriteSchools = new ObservableCollection<School>();

			MessagingCenter.Subscribe<StartUpPage, School>(this, "choosenSch", (sender, args) =>
			{
				favoriteSchools.Add(args);
			});

			List<List<CalendarDay>> schoolCalendars = new List<List<CalendarDay>>();
			List<CalendarDay> calendarDays = new List<CalendarDay>();

			IEnumerator<School> enumerator = favoriteSchools.GetEnumerator();

			int i = 0;
			while (enumerator.MoveNext())
			{
				School school = enumerator.Current;

			}

			grouped = new ObservableCollection<GroupedVeggieModel>();
			var veggieGroup = new GroupedVeggieModel() { LongName = "vegetables", ShortName = "v" };
			var fruitGroup = new GroupedVeggieModel() { LongName = "fruit", ShortName = "f" };
			veggieGroup.Add(new VeggieModel() { Name = "celery", IsReallyAVeggie = true, Comment = "try ants on a log" });
			veggieGroup.Add(new VeggieModel() { Name = "tomato", IsReallyAVeggie = false, Comment = "pairs well with basil" });
			veggieGroup.Add(new VeggieModel() { Name = "zucchini", IsReallyAVeggie = true, Comment = "zucchini bread > bannana bread" });
			veggieGroup.Add(new VeggieModel() { Name = "peas", IsReallyAVeggie = true, Comment = "like peas in a pod" });
			fruitGroup.Add(new VeggieModel() { Name = "banana", IsReallyAVeggie = false, Comment = "available in chip form factor" });
			fruitGroup.Add(new VeggieModel() { Name = "strawberry", IsReallyAVeggie = false, Comment = "spring plant" });
			fruitGroup.Add(new VeggieModel() { Name = "cherry", IsReallyAVeggie = false, Comment = "topper for icecream" });
			grouped.Add(veggieGroup); grouped.Add(fruitGroup);
			lstView.ItemsSource = grouped;

			//lstView.Children.ElementAt(0).BackgroundColor = Color.Red;
			//lstView.Children.BackgroundColor = Color.Red;
			//labeltest.
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
