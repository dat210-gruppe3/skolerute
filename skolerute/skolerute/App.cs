using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace skolerute
{
    public class App : Application
    {
		public App()
        {
			// The root page of your application
			MainPage = new Views.TabPage();	
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
			string url = "http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/32d52130-ce7c-4282-9d37-3c68c7cdba92/download/skolerute-2016-17.csv";
			db.DatabaseManager database = new db.DatabaseManager();
			database.DeleteDatabase();
			database.CreateNewDatabase();

			//List<data.School> SKOLENE = database.GetSchools().ToList();
			skolerute.db.CSVParser parser = new db.CSVParser(url, database);
			await parser.StringParser(url);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
