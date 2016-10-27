using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using skolerute.data;

namespace skolerute.Views
{
	public partial class ListPage : ContentPage
	{	//FØLGENDE LINJE ER DEL AV EKSEMPELKODE
		private ObservableCollection<GroupedFreeDayModel> grouped { get; set; }
		ObservableCollection<School> favoriteSchools = new ObservableCollection<School>();


		public ListPage()
		{
			MessagingCenter.Subscribe<StartUpPage, School>(this, "choosenSch", (sender, args) =>
			{
				favoriteSchools.Add(args);
			});
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

            /*
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
            */
            //lstView.Children.ElementAt(0).BackgroundColor = Color.Red;
            //lstView.Children.BackgroundColor = Color.Red;
            //labeltest.


        }

        protected override async void OnAppearing()
        {
            

            List<List<CalendarDay>> schoolCalendars = new List<List<CalendarDay>>();
            List<CalendarDay> calendarDays = new List<CalendarDay>();

            //TODO remove Placeholder
            School skole = favoriteSchools[0];
            db.DatabaseManagerAsync database = new db.DatabaseManagerAsync();

            //School skole = await database.GetSchool(1);


            grouped = new ObservableCollection<GroupedFreeDayModel>();
            GroupedFreeDayModel FreeDayGroup = null;

            for (int i = 0; i < skole.calendar.Count; i++)
            {
                CalendarDay currentDay = skole.calendar[i];
                //Ignore weekends
                if(skole.calendar[i].date.DayOfWeek == DayOfWeek.Saturday || skole.calendar[i].date.DayOfWeek == DayOfWeek.Sunday)
                {
                    i++;
                }
                //Handle vacations
                else if(currentDay.comment.Substring(Math.Max(0, currentDay.comment.Length - 5)) == "ferie")
                {
                    FreeDayGroup = new GroupedFreeDayModel() { LongName = currentDay.comment };
                    DateTime startDate = skole.calendar[i].date;
                    string dateInterval;

                    if (i + 1 < skole.calendar.Count)
                    {
                        while (skole.calendar[i].isFreeDay)
                        {
                            i++;

                            if (i >= skole.calendar.Count - 1)
                            {
                                break;
                            }
                        }
                    }

                    DateTime endDate = skole.calendar[i].date;
                    //Check if end of dataset is reached
                    if (i == skole.calendar.Count - 1)
                    {
                        dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;
                    }
                    else
                    {
                        dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year
                            + " - " + endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                    }

                    FreeDayGroup.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                    grouped.Add(FreeDayGroup);
                }
                //Handle summer vacation
                else if (skole.calendar[i].isFreeDay && skole.calendar[i].comment == "")
                {
                    FreeDayGroup = new GroupedFreeDayModel() { LongName = "Sommerferie" };
                    DateTime startDate = skole.calendar[i].date;
                    string dateInterval;

                    if (i + 1 < skole.calendar.Count)
                    {
                        while (skole.calendar[i].isFreeDay)
                        {
                            i++;

                            if (i >= skole.calendar.Count - 1)
                            {
                                break;
                            }
                        }
                    }

                    DateTime endDate = skole.calendar[i].date;
                    //Check if end of dataset is reached
                    if (i == skole.calendar.Count-1)
                    {
                        FreeDayGroup.LongName = "Starten av sommerferien";
                        dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;
                    } else
                    {
                        FreeDayGroup.LongName = "Slutten av sommerferien";
                        dateInterval = " - " + endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                    }

                    FreeDayGroup.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                    grouped.Add(FreeDayGroup);
                }
                //Handle other types of freedays
                else if (skole.calendar[i].isFreeDay)
                {
                    FreeDayGroup = new GroupedFreeDayModel() { LongName = skole.calendar[i].comment };
                    
                    string currentComment = skole.calendar[i].comment;
                    DateTime startDate = skole.calendar[i].date;

                    /*
                    if(i+1 < skole.calendar.Count)
                    {
                        while (skole.calendar[i + 1].comment == currentComment)
                        {
                            i++;

                            if (i >= skole.calendar.Count - 1)
                            {
                                break;
                            }
                        }
                    } 

                    DateTime endDate = skole.calendar[i].date;
                    */
                    string dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;

                    FreeDayGroup.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                    grouped.Add(FreeDayGroup);
                }
                
            }
            lstView.ItemsSource = grouped;
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
