using skolerute.data;
using skolerute.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using skolerute.notifications;

using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class StartUpPage : ContentPage
    {
        List<School> debugskoler = new List<School>();
        ObservableCollection<string> mySchools= new ObservableCollection<string>();
        List<double> avstander = new List<double>();
        List<int> bestMatches = new List<int>();


        public StartUpPage()
        {
			InitializeComponent();          
        }

        protected override async void OnAppearing()
        {

            

            base.OnAppearing();
            if (skoler.ItemsSource == null) { 
		        debugskoler = await GetListContent();
                mineskoler.ItemsSource = mySchools;
            }
            else
            {
                skoler.ItemsSource = debugskoler;
            }
            if (SettingsManager.GetPreference("i") != null && (bool)SettingsManager.GetPreference("i"))
            {
                mySchools = await getFavSchools();
                mineskoler.ItemsSource = mySchools;
			}


            DependencyService.Get<notifications.INotification>().SendCalendarNotification("title", "desc", DateTime.Now);


            
            
            // Tar inn gps-latitude(double),gps-longitude(double),liste med skolenes latitude(double),liste med skolenes longitudes(double)
            // Lagrer liste med skoleid på de 5 nærmeste skolene. (bestMatches)
            // Lagrer liste med avstand i km samsvarende med listen av skoleid-er. (avstander) Mikser litt norsk og engelsk her ser jeg:)
            //getNearbySchools(gpslat,gpslong,latitudes,longitudes);       

        }
        
        private void GetClosest()
        {
            List<double> userposition = DependencyService.Get<GPS.IGPSservice>().GetGpsCoordinates();
            List<data.School> newSchList = new List<data.School>();
        }

        private void TextChanged(Object o, EventArgs e)
        {
            // Called in xaml: When the searchbar text changes,
            // check if any of the school names contains a substring equal to current searchtext, 
            // and display them if they do, if not, display nothing, if string is empty, display all
            List<data.School> newSchList = new List<data.School>();
            var sValue = searchSchool.Text.Trim();
            if (sValue == "")
            {
                skoler.ItemsSource = debugskoler;
            }
            else
            {
                for (int i = 0; i < debugskoler.Count; i++)
                {
                    if (debugskoler[i].name.Contains(sValue) || debugskoler[i].name.ToLower().Contains(sValue))
                    {
                        newSchList.Add(debugskoler[i]);
                    }
                }
                skoler.ItemsSource = newSchList;
            }
        }

        private async Task<List<School>> GetListContent()
        {
            db.DatabaseManagerAsync database = new db.DatabaseManagerAsync();

            InitializeComponent();
            var progressBar = this.FindByName<ProgressBar>("progressBar");
            progressBar.IsVisible = true;

            if (! await db.DatabaseManagerAsync.TableExists<School>(database.connection) || ! await db.DatabaseManagerAsync.TableExists<CalendarDay>(database.connection))
            {
                await progressBar.ProgressTo(0.3, 100, Easing.Linear);
                database.CreateNewDatabase();
                skolerute.db.CSVParser parser = new db.CSVParser(Constants.URL, database);
                //await parser.StringParser();
                await parser.RetrieveSchools();
                await progressBar.ProgressTo(0.7, 250, Easing.Linear);
            }

            List<School> debugskoler = new List<School>();

            try
            {
                debugskoler = await database.GetSchools();
                await progressBar.ProgressTo(1, 250, Easing.Linear);
                skoler.ItemsSource = debugskoler;
                progressBar.IsVisible = false;
                return debugskoler;
            }
            catch (Exception e)
            {
                List<School> lista = new List<School>();
                lista.Add(new School(e.Message, null));
                skoler.ItemsSource = lista;
                await DisplayActionSheet("En feil oppstod i GetListContent()","","");
                progressBar.IsVisible = false;
                return lista;
            }
        }

        // Event som blir trigga når man trykker på en skole i lista
        public async void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            School skole = (School)e.SelectedItem;
            string skolenavn = skole.name;

            var list = (ListView)sender;
            
            var action = await DisplayActionSheet("Du valgte: " + skolenavn, "Legg til", "Avbryt");

            if (action != null)
            {
                string actionNavn = action.ToString();
                if (actionNavn == "Legg til")
                {
                    // Hent kalenderen til valgt skole
                    if (!mySchools.Contains(skolenavn))
                    {

                        mySchools.Add(skolenavn);
                        mineskoler.ItemsSource = mySchools;
                        if (SettingsManager.GetPreference("i") == null)
                        {
                            await SettingsManager.SavePreferenceAsync("i", true);
                            foreach (School x in debugskoler)
                            {
                                await SettingsManager.SavePreferenceAsync(x.ID.ToString(), false);
                            }
                            await SettingsManager.SavePreferenceAsync(skole.ID.ToString(), true);
                        }
                        else
                        {
                            await SettingsManager.SavePreferenceAsync(skole.ID.ToString(), true);
                            
                        }

                    }
                    
                    db.DatabaseManagerAsync database = new db.DatabaseManagerAsync();
                    skolerute.db.CSVParser parser = new db.CSVParser(Constants.URL, database);
                    await parser.RetrieveCalendar(skole);
                    MessagingCenter.Send<StartUpPage, School>(this, "choosenSch", skole);

                }
            }

            else
            {

            } 

            
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
                else
                {

                }
            }

        private async Task<ObservableCollection<string>> getFavSchools()
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            db.DatabaseManagerAsync db = new db.DatabaseManagerAsync();
            foreach (School x in debugskoler)
            {
                if ((bool)SettingsManager.GetPreference(x.ID.ToString())) { 
                    School currentschool = await db.GetSchool(x.ID);
                    currentschool.calendar = await db.GetOnlyCalendar(x.ID);
                    list.Add(currentschool.name);
                    MessagingCenter.Send<StartUpPage, School>(this, "choosenSch", currentschool);
                }
            }
            return list;
        }

        private void getNearbySchools(double gpslati, double gpslongi)
        {

            List<double> latitudes = new List<double>();
            List<double> longitudes = new List<double>();

            latitudes.Add(gpslati);
            longitudes.Add(gpslongi);

            foreach (School x in debugskoler)
            {
                latitudes.Add(x.latitude);
                longitudes.Add(x.longitude);
            }

            for (int i=1;i<latitudes.Count;i++)
            {
                if (i<6)
                {
                    bestMatches.Add(i);
                    avstander.Add(DistanceCalc.HaversineDistance(latitudes.ElementAt(0), longitudes.ElementAt(0), latitudes.ElementAt(i), longitudes.ElementAt(i)));
                }  
                
                else
                {
                    double biggest = avstander.Max();
                    int bigindex = avstander.FindIndex(0, 5, y => y == biggest);
                    double ny = DistanceCalc.HaversineDistance(latitudes.ElementAt(0), longitudes.ElementAt(0), latitudes.ElementAt(i), longitudes.ElementAt(i));

                    if (ny < avstander.ElementAt(bigindex))
                    {
                        avstander.RemoveAt(bigindex);
                        avstander.Insert(bigindex, ny);
                        bestMatches.RemoveAt(bigindex);
                        bestMatches.Insert(bigindex,i);      
                    }  
                }           
            }
        }
    }
    }

