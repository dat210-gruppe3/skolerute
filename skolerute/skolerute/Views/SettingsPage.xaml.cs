using System;
using System.Threading.Tasks;
using skolerute.data;
using skolerute.utils;
using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            // Checking to see if the value of offline mode has been set by the user
            if(SettingsManager.GetPreference(Constants.OfflineMode) != null)
            {
                offlineMode.SetValue(SwitchCell.OnProperty, SettingsManager.GetPreference(Constants.OfflineMode));
            }
        }

		//protected override void OnAppearing()
		//{
		//	string DBUG = "hei";
		//	//DependencyService.Get<INotification>().SendCalendarNotification("testTittel", "dette er en test", DateTime.Now.AddSeconds(10));
		//	//DependencyService.Get<notifications.INotification>().SendCalendarNotification("title", "desc", DateTime.Now.AddSeconds(10));

		//}

        async Task OnOfflineModeChanged(object sender, EventArgs ea)
        {
            //TODO: Check to see whether or not offline mode has been set in the past, if not set it to false when the
            // app starts
            if (offlineMode.On == true)
            {
                await SettingsManager.SavePreferenceAsync(Constants.OfflineMode, true);
				//TODO: Start parsing all data from csv file into database
				//DependencyService.Get<notifications.INotification>().SendCalendarNotification("title", "desc", DateTime.Now.AddSeconds(10));
            }
            else if(!offlineMode.On)
            {
                await SettingsManager.SavePreferenceAsync(Constants.OfflineMode, false);
            }
        }
    }

}
