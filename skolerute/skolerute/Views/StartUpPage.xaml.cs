using skolerute.data;
using skolerute.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using skolerute.GPS;
using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class StartUpPage : ContentPage
    {
        List<WrappedListItems<School>> WrappedItems = new List<WrappedListItems<School>>();

        public StartUpPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            

            List<School> allSchools = new List<School>();

            if (schools.ItemsSource == null)
            {
                allSchools = await GetListContent();
                WrappedItems = allSchools.Select(item => new WrappedListItems<School>() { Item = item, IsChecked = false, UnChecked = true, Distance = 0, DistanceVisible = false }).ToList();
                schools.ItemsSource = WrappedItems;
            }

            if (SettingsManager.GetPreference("i") != null && (bool)SettingsManager.GetPreference("i"))
            {
                ObservableCollection<string> mySchools = await GetFavSchools();
                foreach (string school in mySchools) {
                    WrappedListItems<School> temp = WrappedItems.First(item => item.Item.name == school);
                    temp.IsChecked = true;
                    temp.UnChecked = false;
                }   
            }

            MessagingCenter.Subscribe<Coordinate>(this, "locationUpdate", (args) =>
            {
                loadingNearbySchools.IsVisible = false;
                loadingNearbySchools.IsRunning = false;
                schools.IsVisible = true;
                schools.IsRefreshing = false;
                schools.ItemsSource = GPS.GPSservice.GetNearbySchools(WrappedItems);
            });

            //DependencyService.Get<notifications.INotification>().SendCalendarNotification("title", "desc", DateTime.Now);
        }

        private void PullRefresher()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                DependencyService.Get<GPS.IGPSservice>().ConnectGps();
            }

            else
            {
                schools.ItemsSource = GPS.GPSservice.GetNearbySchools(WrappedItems);
            }
        }

        private void GetClosest()
        {
            //Called in xaml if button to get closest schools is pressed. Gets user global position and compares it
            //to school positions and displays these schools in the GUI school list.

            if (GetCoords.Text == "Vis nærmeste")
            {
				GetCoords.Text = "Vis alle";
                if (Device.OS == TargetPlatform.Android)
                {
                    schools.IsVisible = false;
                    loadingNearbySchools.IsVisible = true;
                    loadingNearbySchools.IsRunning = true;
                    DependencyService.Get<GPS.IGPSservice>().ConnectGps();
                }
                else
                {
                List<WrappedListItems<School>> newWrappedItems = GPS.GPSservice.GetNearbySchools(WrappedItems);
                if(newWrappedItems == null) { return; }
                schools.ItemsSource = newWrappedItems;
                }
            }
            else
            {
                GetCoords.Text = "Vis nærmeste";
                foreach(WrappedListItems<School> item in WrappedItems)
                {
                    item.DistanceVisible = false;
                }
                schools.ItemsSource = WrappedItems;
            }
        }

        private void TextChanged(Object o, EventArgs e)
        {
            // Called in xaml: When the searchbar text changes,
            // check if any of the school names contains a substring equal to current searchtext, 
            // and display them if they do, if not, display nothing, if string is empty, display all

            List<WrappedListItems<School>> newSchList = new List<WrappedListItems<School>>();
            if (searchSchool.Text == null)
            {
                schools.ItemsSource = WrappedItems;
                return;
            }

            var sValue = searchSchool.Text.Trim();

            if (sValue == "")
            {
                schools.ItemsSource = WrappedItems;
                if (Device.OS == TargetPlatform.iOS)
                {
                    searchSchool.Unfocus();
                }
            }
            else
            {
                for (int i = 0; i < WrappedItems.Count; i++)
                {
                    if (WrappedItems[i].Item.name.ToLower().Contains(sValue) || WrappedItems[i].Item.name.Contains(sValue))
                    {
                        newSchList.Add(WrappedItems[i]);
                    }
                }
                schools.ItemsSource = newSchList;
            }
        }

        private async Task<List<School>> GetListContent()
        {
            db.DatabaseManagerAsync database = new db.DatabaseManagerAsync();

            InitializeComponent();
            var progressBar = this.FindByName<ProgressBar>("progressBar");
            progressBar.IsVisible = true;

            if (!await db.DatabaseManagerAsync.TableExists<School>(database.connection) ||
                !await db.DatabaseManagerAsync.TableExists<CalendarDay>(database.connection))
            {
                await progressBar.ProgressTo(0.3, 100, Easing.Linear);
                database.CreateNewDatabase();
                skolerute.db.CSVParser parser = new db.CSVParser(Constants.URL, database);

                // TODO: Try and catch this line
                await parser.RetrieveSchools();
                await progressBar.ProgressTo(0.7, 250, Easing.Linear);
            }

            List<School> allSchools = new List<School>();

            try
            {
                allSchools = await database.GetSchools();
                await progressBar.ProgressTo(1, 250, Easing.Linear);
                progressBar.IsVisible = false;
                GetCoords.IsEnabled = true;
                return allSchools;
            }
            catch (System.Net.WebException e)
            {
                await DisplayAlert("Internett problemer", "Kunne ikke hente ned skolene, prøv igjen senere.", "Ok");
                return null;
            }
            catch (Exception e)
            {
                await DisplayActionSheet("Feil", "En feil oppstod, prøv igjen.", "Ok");
                progressBar.IsVisible = false;
                return null;
            }
        }

        // Event triggered when a school in the list is selected
        public async void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            string skolenavn = "";
            School school = null;

            if ((e.SelectedItem).GetType() == typeof(string))
            {
                skolenavn = (string)(e.SelectedItem);
                int index = skolenavn.IndexOf(':');
                skolenavn = skolenavn.Remove(index, (skolenavn.Length) - index);
                school = WrappedItems.Find(y => y.Item.name == skolenavn).Item;
            }
            else
            {
                WrappedListItems<School> item = (WrappedListItems<School>)e.SelectedItem;
                school = item.Item;
                skolenavn = school.name;
            }

            // Get calendar for chosen school
            WrappedListItems<School> chSchool = WrappedItems.Find(item => item.Item.name == skolenavn);
            if (chSchool.IsChecked)
            {
                chSchool.IsChecked = false;
                chSchool.UnChecked = true;
                MessagingCenter.Send<StartUpPage, string>(this, "deleteSch", chSchool.Item.name);
            } else
            {
                chSchool.IsChecked = true;
                chSchool.UnChecked = false;

                if (SettingsManager.GetPreference("i") == null)
                {
                    await SettingsManager.SavePreferenceAsync("i", true);
                    foreach (School x in WrappedItems.Select(item => item.Item).ToList())
                    {
                        await SettingsManager.SavePreferenceAsync(x.ID.ToString(), false);
                    }
                    await SettingsManager.SavePreferenceAsync(school.ID.ToString(), true);
                }
                else
                {
                    await SettingsManager.SavePreferenceAsync(school.ID.ToString(), true);
                }

                MessagingCenter.Send(this, "newSchoolSelected");
                db.DatabaseManagerAsync database = new db.DatabaseManagerAsync();
                db.CSVParser parser = new db.CSVParser(Constants.URL, database);

                try
                {
                    await parser.RetrieveCalendar(school);
                    MessagingCenter.Send(this, "choosenSch", school);
                }
                catch (System.Net.WebException exception)
                {
                    await DisplayAlert("Problem med internett", "Kunne ikke laste ned skoleruten, prøv igjen senere", "Ok");
                }
            }

            ((ListView)sender).SelectedItem = null;
    }

        private async Task<ObservableCollection<string>> GetFavSchools()
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            db.DatabaseManagerAsync db = new db.DatabaseManagerAsync();

            School x;
            for (int i = 0; i < WrappedItems.Count; i++)
            {
                x = WrappedItems[i].Item;
                if ((bool)SettingsManager.GetPreference(x.ID.ToString()))
                {
                    School currentschool = await db.GetSchool(x.ID);
                    currentschool.calendar = await db.GetOnlyCalendar(x.ID);
                    list.Add(currentschool.name);
                    MessagingCenter.Send<StartUpPage, School>(this, "choosenSch", currentschool);
                }
            }
            return list;
        }
    }
}

