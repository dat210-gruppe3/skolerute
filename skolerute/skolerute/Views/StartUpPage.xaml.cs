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
		public StartUpPage()
		{
			//http://blog.alectucker.com/post/2015/08/10/xamarinforms-searchbar-with-mvvm.aspx

			SchoolList skolelisten = Init();
			BindingContext = skolelisten;
			InitializeComponent();
            List<School> lista = new List<School>() { new School("Gosen", null) };
            skoler.ItemsSource = lista;
            List<School> debugskoler = new List<School>();

            try
            {
                db.DatabaseManager database = new db.DatabaseManager();
                debugskoler = database.GetSchools().ToList();
                skoler.ItemsSource = debugskoler;
            } catch (Exception e)
            {
                lista.Add(new School("Error.", null));
                skoler.ItemsSource = lista;
                debugskoler = lista;
            }

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




	}
}
