using skolerute.data;
using skolerute.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class StartUpPage : ContentPage
    {
        List<School> allSchools = new List<School>();
        ObservableCollection<string> mySchools = new ObservableCollection<string>();
        List<double> distances = new List<double>();
        List<int> bestMatches = new List<int>();

        public StartUpPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (schools.ItemsSource == null)
            {
                allSchools = await GetListContent();
                mineskoler.ItemsSource = mySchools;
            }
            else
            {
                schools.ItemsSource = allSchools;
            }
            if (SettingsManager.GetPreference("i") != null && (bool)SettingsManager.GetPreference("i"))
            {
                mySchools = await GetFavSchools();
                mineskoler.ItemsSource = mySchools;

            }


            //DependencyService.Get<notifications.INotification>().SendCalendarNotification("title", "desc", DateTime.Now);
        }

        private async Task Load()
        {
            GetCoords.IsEnabled = false;
            await Task.Yield();
            GetClosest();
            GetCoords.IsEnabled = true;
        }

        private void GetClosest()
        {
            //Called in xaml if button to get closest schools is pressed. Gets user global position and compares it
            //to school positions and displays these schools in the GUI school list.
            if (GetCoords.Text == "Vis nærmeste")
            {               
                Coordinate userposition = DependencyService.Get<GPS.IGPSservice>().GetGpsCoordinates();

                if (userposition == null) return;

                GetNearbySchools(userposition);
                GetCoords.Text = "Vis alle";
                List<School> ads = bestMatches.Select(x => allSchools.Find(y => y.ID == x)).ToList();
                schools.ItemsSource = ads;
                avstand.IsVisible = true;
                List<string> result = new List<string>();
                for (int a = 0; a < distances.Count; a++)
                {
                    string verdi = ads.ElementAt(a).name + ": " + Math.Round(distances.ElementAt(a), 2).ToString() + " km";
                    result.Add(verdi);
                }
                avstand.ItemsSource = result;
                schools.IsVisible = false;
            }
            else
            {
                GetCoords.Text = "Vis nærmeste";
                schools.ItemsSource = allSchools;
                avstand.IsVisible = false;
                schools.IsVisible = true;
            }
        }

        private void TextChanged(Object o, EventArgs e)
        {
            // Called in xaml: When the searchbar text changes,
            // check if any of the school names contains a substring equal to current searchtext, 
            // and display them if they do, if not, display nothing, if string is empty, display all

            List<data.School> newSchList = new List<data.School>();
            if (searchSchool.Text == null)
            {
                schools.ItemsSource = allSchools;
                return;
            }

            var sValue = searchSchool.Text.Trim();

            if (sValue == "")
            {
                schools.ItemsSource = allSchools;
            }
            else
            {
                for (int i = 0; i < allSchools.Count; i++)
                {
                    if (allSchools[i].name.Contains(sValue) || allSchools[i].name.ToLower().Contains(sValue))
                    {
                        newSchList.Add(allSchools[i]);
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

            if (!await db.DatabaseManagerAsync.TableExists<School>(database.connection) || !await db.DatabaseManagerAsync.TableExists<CalendarDay>(database.connection))
            {
                await progressBar.ProgressTo(0.3, 100, Easing.Linear);
                database.CreateNewDatabase();
                skolerute.db.CSVParser parser = new db.CSVParser(Constants.URL, database);

                await parser.RetrieveSchools();
                await progressBar.ProgressTo(0.7, 250, Easing.Linear);
            }

            List<School> allSchools = new List<School>();

            try
            {
                allSchools = await database.GetSchools();
                await progressBar.ProgressTo(1, 250, Easing.Linear);
                schools.ItemsSource = allSchools;
                progressBar.IsVisible = false;
                GetCoords.IsEnabled = true;
                return allSchools;
            }
            catch (Exception e)
            {
                List<School> lista = new List<School>();
                lista.Add(new School(e.Message, null));
                schools.ItemsSource = lista;
                await DisplayActionSheet("En feil oppstod i GetListContent()", "", "");
                progressBar.IsVisible = false;
                return lista;
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
                school = allSchools.Find(y => y.name == skolenavn);
            }
            else
            {
                school = (School)e.SelectedItem;
                skolenavn = school.name;
            }

            var action = await DisplayActionSheet("Du valgte: " + skolenavn, "Legg til", "Avbryt");

            if (action != null)
            {
                string actionNavn = action.ToString();
                if (actionNavn == "Legg til")
                {
                    // Get calendar for chosen school
                    if (!mySchools.Contains(skolenavn))
                    {
                        mySchools.Add(skolenavn);
                        mineskoler.ItemsSource = mySchools;
                        if (SettingsManager.GetPreference("i") == null)
                        {
                            await SettingsManager.SavePreferenceAsync("i", true);
                            foreach (School x in allSchools)
                            {
                                await SettingsManager.SavePreferenceAsync(x.ID.ToString(), false);
                            }
                            await SettingsManager.SavePreferenceAsync(school.ID.ToString(), true);
                        }
                        else
                        {
                            await SettingsManager.SavePreferenceAsync(school.ID.ToString(), true);
                        }
                    }
                    MessagingCenter.Send(this, "newSchoolSelected");
                    db.DatabaseManagerAsync database = new db.DatabaseManagerAsync();
                    db.CSVParser parser = new db.CSVParser(Constants.URL, database);
                    ((ListView)sender).SelectedItem = null;

                    await parser.RetrieveCalendar(school);
                    MessagingCenter.Send(this, "choosenSch", school);

                }
            }
            ((ListView)sender).SelectedItem = null;
        }

        public async void OnDeletion(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            string skolenavn = (string)e.SelectedItem;
            var action = await DisplayActionSheet("Du valgte: " + skolenavn, "Slett", "Avbryt");

            if (action != null)
            {

                string actionNavn = action.ToString();
                if (actionNavn == "Slett")
                {
                    if (mySchools.Contains(skolenavn))
                    {
                        mySchools.Remove(skolenavn);
                        mineskoler.ItemsSource = mySchools;
                    }
                    MessagingCenter.Send<StartUpPage, string>(this, "deleteSch", skolenavn);
                }
            }
            ((ListView)sender).SelectedItem = null;
        }

        private async Task<ObservableCollection<string>> GetFavSchools()
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            db.DatabaseManagerAsync db = new db.DatabaseManagerAsync();

            foreach (School x in allSchools)
            {
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

        private void GetNearbySchools(Coordinate gpsCoordinates)
        {
            distances = new List<double>();
            bestMatches = new List<int>();
            List<double> latitudes = new List<double>();
            List<double> longitudes = new List<double>();

            foreach (School x in allSchools)
            {
                latitudes.Add(x.latitude);
                longitudes.Add(x.longitude);
            }

            for (int i = 0; i < latitudes.Count; i++)
            {
                if (i < 5)
                {
                    bestMatches.Add(i);
                    distances.Add(gpsCoordinates.HaversineDistance(latitudes.ElementAt(i), longitudes.ElementAt(i)));
                }

                else
                {
                    double biggest = distances.Max();
                    int bigindex = distances.FindIndex(0, 5, y => y == biggest);
                    double ny = gpsCoordinates.HaversineDistance(latitudes.ElementAt(i), longitudes.ElementAt(i));

                    if (ny < distances.ElementAt(bigindex))
                    {
                        distances.RemoveAt(bigindex);
                        distances.Insert(bigindex, ny);
                        bestMatches.RemoveAt(bigindex);
                        bestMatches.Insert(bigindex, i + 1);
                    }
                }
            }

            List<double> avs = new List<double>(5);
            List<int> ids = new List<int>(5);
            for (int j = 5; j > 0; j--)
            {
                double min = distances.Min();
                int minindex = distances.FindIndex(0, j, y => y == min);
                avs.Add(distances[minindex]);
                ids.Add(bestMatches[minindex]);
                distances.RemoveAt(minindex);
                bestMatches.RemoveAt(minindex);
            }
            bestMatches = ids;
            distances = avs;
        }

    }
}

