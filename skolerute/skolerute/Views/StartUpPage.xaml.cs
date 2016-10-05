using skolerute.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace skolerute.Views
{
	public partial class StartUpPage : ContentPage
	{
        List<School> debugskoler = new List<School>();

		public StartUpPage()
		{
			//http://blog.alectucker.com/post/2015/08/10/xamarinforms-searchbar-with-mvvm.aspx

			SchoolList skolelisten = Init();
			BindingContext = skolelisten;
			InitializeComponent();

			searchSchool.SearchButtonPressed += (s, e) => {
                List<School> newSchList = new List<School>();
                var sValue = searchSchool.Text.Trim();
                if (sValue == "")
                {
                    skoler.ItemsSource = debugskoler;
                } else { 
                    for (int i = 0; i < debugskoler.Count; i++) 
				    {
                        if (debugskoler[i].name.Contains(sValue))
					    {
						    newSchList.Add(debugskoler[i]);
	        		    }
                    }
				    skoler.ItemsSource = newSchList;
                }
            };


            //Prøver en liten 'autocomplete' basert på samme konsept
            searchSchool.TextChanged += (s, e) =>
            {
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
                        if (debugskoler[i].name.Contains(sValue))
                        {
                            newSchList.Add(debugskoler[i]);
                            skoler.ItemsSource = newSchList;
                        }
                    }
                    
                }
            };

            
		}

        public void ac(object o, TextChangedEventArgs a)
        {

        }

        private SchoolList Init()
		{
			return new data.SchoolList { liste = new List<string>() { "Kannik", "Sandnes", "Kongsgård" } };
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetListContent();
        }

        private async Task<List<School>> GetListContent()
        {
            string url = "http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/32d52130-ce7c-4282-9d37-3c68c7cdba92/download/skolerute-2016-17.csv";

            InitializeComponent();
            var progressBar = this.FindByName<ProgressBar>("progressBar");
            progressBar.IsVisible = true;
            //List<data.School> SKOLENE = database.GetSchools().ToList();
            skolerute.db.CSVParser parser = new db.CSVParser(url);
            await progressBar.ProgressTo(0.3, 100, Easing.Linear);
			try
			{
				await parser.StringParser();
			}
			catch (Exception ex)
			{
				List<School> lista = new List<School>();
				lista.Add(new School("Net error.", null));
				skoler.ItemsSource = lista;
				return lista;
			}
            await progressBar.ProgressTo(0.7, 250, Easing.Linear);
            List<School> debugskoler = new List<School>();

            try
            {
                db.DatabaseManagerAsync database = new db.DatabaseManagerAsync();
                debugskoler = await database.GetSchools();
                await progressBar.ProgressTo(1, 250, Easing.Linear);
                skoler.ItemsSource = debugskoler;
                progressBar.IsVisible = false;
                return debugskoler;
            }
            catch (Exception e)
            {
                List<School> lista = new List<School>();
                lista.Add(new School("Error.", null));
                skoler.ItemsSource = lista;
                return lista;
            }
        }
	}
}
