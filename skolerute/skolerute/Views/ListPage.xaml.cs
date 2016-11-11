using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using skolerute.data;

namespace skolerute.Views
{
    public partial class ListPage : ContentPage
    {
        private ObservableCollection<GroupedFreeDayModel> grouped { get; set; }
        List<School> favoriteSchools = new List<School>();
        List<string> favoriteSchoolNames = new List<string>();

        public ListPage()
        {
            MessagingCenter.Subscribe<StartUpPage, School>(this, "choosenSch", (sender, args) =>
            {
                if(!favoriteSchoolNames.Contains(args.name))
                {
                    favoriteSchools.Add(args);
                    favoriteSchoolNames.Add(args.name);
                    AddSchoolToList(favoriteSchools);

                    for(int i = 0; i < grouped.Count; i++)
                    {
                        //Check if current freeperiod is not the same for all schools
                        if (grouped[i].Any(o => o.Comment != grouped[i][0].Comment))
                        {
                            continue;
                        }
                        string tempDate = grouped[i][0].Comment;
                        grouped[i].Clear();
                        grouped[i].Add(new FreeDayModel() { Name = "Alle valgte skoler", Comment = tempDate });
                    }

                    lstView.ItemsSource = grouped;
                }
            });

            MessagingCenter.Subscribe<StartUpPage, string>(this, "deleteSch", (sender, args) =>
            {
                favoriteSchools.Remove(favoriteSchools.Find(x => x.name.Contains(args)));
                favoriteSchoolNames.Remove(args);
                grouped.Clear();
                AddSchoolToList(favoriteSchools);
                lstView.ItemsSource = grouped;
            });

            InitializeComponent();

        }

        public void AddSchoolToList(List<School> skoler)
        {
            GroupedFreeDayModel FreeDayGroup = null;
            grouped = new ObservableCollection<GroupedFreeDayModel>();
            string dateInterval = "";

            foreach (School skole in favoriteSchools)
            {
                for (int i = 0; i < skole.calendar.Count; i++)
                {
                    CalendarDay currentDay = skole.calendar[i];
                    FreeDayGroup = null;
                    //Ignore weekends
                    if (skole.calendar[i].Date.DayOfWeek == DayOfWeek.Saturday || skole.calendar[i].Date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        i++;
                    }
                    //Handle vacations
                    else if (currentDay.Comment.Substring(Math.Max(0, currentDay.Comment.Length - 5)) == "ferie")
                    {
                        FreeDayGroup = new GroupedFreeDayModel() { LongName = currentDay.Comment, ShortName="" };
                        DateTime startDate = skole.calendar[i].Date;

                        if (i + 1 < skole.calendar.Count)
                        {
                            while (skole.calendar[i].IsFreeDay)
                            {
                                i++;

                                if (i >= skole.calendar.Count - 1)
                                {
                                    break;
                                }
                            }
                        }

                        DateTime endDate = skole.calendar[i].Date;
                        dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year
                            + " - " + endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                    }
                    //Handle summer vacation
                    else if (skole.calendar[i].IsFreeDay && skole.calendar[i].Comment == "")
                    {
                        FreeDayGroup = new GroupedFreeDayModel() { LongName = "Sommerferie", ShortName="" };
                        DateTime startDate = skole.calendar[i].Date;

                        if (i + 1 < skole.calendar.Count)
                        {
                            while (skole.calendar[i].IsFreeDay)
                            {
                                i++;

                                if (i >= skole.calendar.Count - 1)
                                {
                                    break;
                                }
                            }
                        }

                        DateTime endDate = skole.calendar[i].Date;
                        //Check if end of dataset is reached
                        if (i == skole.calendar.Count - 1)
                        {
                            FreeDayGroup.LongName = "Starten av sommerferien";
                            dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year + " -";
                        }
                        else
                        {
                            FreeDayGroup.LongName = "Slutten av sommerferien";
                            dateInterval = " - " + endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
                        }
                    }
                    //Handle other types of freedays
                    else if (skole.calendar[i].IsFreeDay)
                    {
                        string currentComment = skole.calendar[i].Comment;
                        DateTime startDate = skole.calendar[i].Date;
                        dateInterval = startDate.Day + "/" + startDate.Month + "/" + startDate.Year;
                        FreeDayGroup = new GroupedFreeDayModel() { LongName = skole.calendar[i].Comment, ShortName = "", Date = dateInterval };
                    }

                    if (FreeDayGroup != null)
                    {
                        //Check if group is already created and add to it if it is
                        bool foundGroup = false;
                        foreach (GroupedFreeDayModel group in grouped)
                        {
                            if (FreeDayGroup.LongName == group.LongName && FreeDayGroup.Date == group.Date)
                            {
                                group.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                                foundGroup = true;
                            }
                        }

                        //If a group was not found then create a new one
                        if (!foundGroup)
                        {
                            FreeDayGroup.Add(new FreeDayModel() { Name = skole.name, Comment = dateInterval });
                            grouped.Add(FreeDayGroup);
                        }
                    }
                }
            }
        }

        protected override async void OnAppearing()
        {

        }
    }
}