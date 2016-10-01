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
			//skoler.ItemsSource = new List<data.School>() {new data.School("Gosen", null)};
			db.DatabaseManager database = new db.DatabaseManager();
			List<data.School> debugskoler = database.GetSchools().ToList();
			skoler.ItemsSource = debugskoler;

			//Dette krasjer programmet totalt.
			//if (searchSchool.IsFocused)
			//{
			//	skoler.ItemsSource = debugskoler;
			//}

			searchSchool.SearchButtonPressed += (s, e) => {
                List<data.School> newSchList = new List<data.School>();
                var sValue = searchSchool.Text;
				for (int i = 0; i < debugskoler.Count; i++) 
				{
                    if (debugskoler[i].name.StartsWith(sValue))
					{
						newSchList.Add(debugskoler[i]);
	        		}
                    else if (debugskoler[i].name.Contains(sValue))
                    {
                        newSchList.Add(debugskoler[i]);
                    }
                }
				skoler.ItemsSource = newSchList;
			};





		}

		private SchoolList Init()
		{
			return new data.SchoolList { liste = new List<string>() { "Kannik", "Sandnes", "Kongsgård" } };
		}




	}
}
