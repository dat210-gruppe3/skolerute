using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace skolerute.Views
{
	public partial class ListPage : ContentPage
	{
		public ListPage()
		{
			InitializeComponent();
			var layout = new StackLayout();
			this.Content = layout;

			//for alle valgte skoler (max. 3), så skal følgende linjer med kode kjøres
			var schLabel = new Label { Text = "Gosen skole", FontSize = 30, HorizontalOptions= LayoutOptions.CenterAndExpand };
			layout.Children.Add(schLabel);
			DateTime date = new DateTime(2001, 1, 1);

			var freeDay = new Label { Text = dayOfWeek(date) + " " + date.ToString("dd.MM.yyyy"), FontSize = 15, HorizontalOptions = LayoutOptions.CenterAndExpand };
			layout.Children.Add(freeDay);
		}

		public static string dayOfWeek(DateTime dt)
		{
			string dow = "";
			switch (dt.DayOfWeek)
			{
				case DayOfWeek.Monday:
					dow = "Mandag";
					break;
				case DayOfWeek.Tuesday:
					dow = "Tirsdag";
					break;
				case DayOfWeek.Wednesday:
					dow = "Onsdag";
					break;
				case DayOfWeek.Thursday:
					dow = "Torsdag";
					break;
				case DayOfWeek.Friday:
					dow = "Fredag";
					break;
				case DayOfWeek.Saturday:
					dow = "Lørdag";
					break;
				case DayOfWeek.Sunday:
					dow = "Søndag";
					break;
			}
			return dow;
		}
	}
}
