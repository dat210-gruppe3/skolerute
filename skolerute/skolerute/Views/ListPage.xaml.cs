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
			var title = new Label { Text = "Neste fridag", FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand };
			layout.Children.Add(title);


			//DateTime[] dager = new DateTime[4] { new DateTime(2017,01,02), new DateTime(2017,04,11), new DateTime(2017,05,15), new DateTime(2017,05,08) };
			//string[] skoler = new string[3] {"Gosen skole", "Tjennsvoll skole", "Austvoll skole"};
			//List<object> liste = new List<object>();
			//for (int i = 0; i < dager.Length; i++) {
			//	liste.Add(dager[i]);
			//	for (int j = 0; j < skoler.Length; j++) { 
			//		liste.Add(skoler[j]);
			//	}
			//}
			//foreach (object i in liste) {
			//	if (i is DateTime) {
			//	}
			//}
			//ListView listview = new ListView { ItemsSource= liste, BackgroundColor = Color.Blue };
			//layout.Children.Add(listview);



		}


	}
}
