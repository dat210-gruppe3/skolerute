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
			SchoolList skolelisten = Init();
			BindingContext = skolelisten;
			InitializeComponent();

			skoler.ItemsSource = new List<data.School>() {new data.School("Gosen", null)};
		}

		private SchoolList Init()
		{
			return new data.SchoolList { liste = new List<string>() { "Kannik", "Sandnes", "Kongsgård" } };
		}

	}
}
