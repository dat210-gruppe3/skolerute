using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using skolerute.data;
using Calendar = skolerute.data.Calendar;

namespace skolerute.Views
{
    public partial class ListPage : ContentPage
    {
        private ObservableCollection<GroupedFreeDayModel> grouped { get; set; }
        List<School> favoriteSchools = new List<School>();
        List<string> favoriteSchoolNames = new List<string>();
        DateTime today = DateTime.Now;
        bool listUpdated = false;

        public ListPage()
        {
            MessagingCenter.Subscribe<StartUpPage, List<School>>(this, "listChanged", (sender, args) =>
            {
                //if (!favoriteSchoolNames.Contains(args.name))
                //{  
                favoriteSchools = args;
                    //favoriteSchoolNames.Add(args.name);
                listUpdated = true;
                    //grouped = Calendar.AddSchoolToList(favoriteSchools);

                    //lstView.ItemsSource = grouped;
                    //FindNext();
                //}

            });

            /*
            MessagingCenter.Subscribe<StartUpPage, string>(this, "deleteSch", (sender, args) =>
            {
                //favoriteSchools.Remove(favoriteSchools.Find(x => x.name.Contains(args)));
                favoriteSchoolNames.Remove(args);
                listUpdated = true;
                //grouped.Clear();
                //grouped = Calendar.AddSchoolToList(favoriteSchools);
                //lstView.ItemsSource = grouped;
                //FindNext();
            });
            */

            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(listUpdated)
            {
                grouped = Calendar.AddSchoolToList(favoriteSchools);
                lstView.ItemsSource = grouped;
                listUpdated = false;
            }

            FindNext();
        }

        private void FindNext()
        {
            
            ObservableCollection<GroupedFreeDayModel> a = lstView.ItemsSource as ObservableCollection<GroupedFreeDayModel>;
            if (a == null) return;
            var cont = true;
            foreach (var group in a)
            {
                foreach (var item in group)
                {
                    try
                    {
                        if (item.GetEndDate() >= DateTime.Now && cont)
                        {
                            lstView.ScrollTo(a[0][0], a[0], ScrollToPosition.Center, false);
                            lstView.ScrollTo(item, group, ScrollToPosition.Center, true);
                            cont = false;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                
            }
            
        }
    }
}