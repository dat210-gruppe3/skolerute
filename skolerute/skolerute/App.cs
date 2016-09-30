using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace skolerute
{
    public class App : Application
    {
        public App()
        {



			//List<data.School> SKOLENE = database.GetSchools().ToList();


			// The root page of your application
			MainPage = new Views.StartUpPage();

			//MainPage = new MainPage();
			//MainPage = new ContentPage
			//{
			//	Content = new StackLayout
			//	{
			//		VerticalOptions = LayoutOptions.Center,
			//		Children = {
			//			 new Label {
			//				HorizontalTextAlignment = TextAlignment.Center,
			//				Text = "Welcome to Xamarin Forms! \n We break shit."
			//			}
			//		}

			//	}
			//};
				
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
