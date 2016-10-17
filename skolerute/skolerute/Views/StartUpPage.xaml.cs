using skolerute.data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class StartUpPage : ContentPage
    {
        List<School> debugskoler = new List<School>();
        List<School> favorites = new List<School>();

        public StartUpPage()
        {
			InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
			if (dineskoler.ItemsSource == null)
			{
				debugskoler = await GetListContent();
                dineskoler.ItemsSource = favorites;
			}
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
                await parser.StringParser();
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
			int skoleId = skole.ID;

			var action = await DisplayActionSheet("Du valgte: " + skolenavn,"Legg til","Avbryt");

            // Henter ut navnet på action (Legg til/Avbryt)
            string actionNavn = action.ToString();

			// Tenker her å kjøre en metode som sjekker actionNavn og hvis "Legg til" går inn i databasen
			// og setter True på den valgte skolens IsFavorite-atributt i databasen.

			if (actionNavn == "Legg til")
			{
                favorites.Add(skole);
				MessagingCenter.Send<StartUpPage, School>(this, "choosenSch", skole);
			}

        }
    }
}
