using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    grouped = Calendar.AddSchoolToList(favoriteSchools);

                    lstView.ItemsSource = grouped;
                }

            });

            MessagingCenter.Subscribe<StartUpPage, string>(this, "deleteSch", (sender, args) =>
            {
                favoriteSchools.Remove(favoriteSchools.Find(x => x.name.Contains(args)));
                favoriteSchoolNames.Remove(args);
                grouped.Clear();
                grouped = Calendar.AddSchoolToList(favoriteSchools);
                lstView.ItemsSource = grouped;
            });

            InitializeComponent();

        }
    }
}