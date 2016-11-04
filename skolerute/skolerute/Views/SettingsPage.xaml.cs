using System;
using System.Threading.Tasks;
using skolerute.utils;
using skolerute.notifications;

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

			// Checking to see if the value of notification has been set by the user
			if (SettingsManager.GetPreference(Constants.Notify) != null)
			{
				notification.SetValue(SwitchCell.OnProperty, SettingsManager.GetPreference(Constants.Notify));
				
			}

			// Add days in picker (user chooses when to get notified)
			for (int i = 1; i < 31; i++)
			{
				daysBeforeNotification.Items.Add(i.ToString());
			}

			//https://developer.xamarin.com/api/type/Xamarin.Forms.Picker/
			daysBeforeNotification.SelectedIndexChanged += (sender, args) =>
				{
					if (daysBeforeNotification.SelectedIndex == -1)
					{
						//boxView.Color = Color.Default;
						string DBUG = "e";
					}
					else
					{
					string notificationDaySelected = daysBeforeNotification.Items[daysBeforeNotification.SelectedIndex];
						//boxView.Color = nameToColor[colorName];
					}
				};


        }

        async Task OnOfflineModeChanged(object sender, EventArgs ea)
        {
            //TODO: Check to see whether or not offline mode has been set in the past, if not set it to false when the
            // app starts
            if (offlineMode.On == true)
            {
                await SettingsManager.SavePreferenceAsync(Constants.OfflineMode, true);
				//TODO: Start parsing all data from csv file into database
            }
            else if(!offlineMode.On)
            {
                await SettingsManager.SavePreferenceAsync(Constants.OfflineMode, false);
            }
        }

		async Task OnNotificationChanged(object sender, EventArgs ea)
		{
			if (notification.On == true)
			{
				DependencyService.Get<INotification>().SendCalendarNotification("No tittel på ios", "dette er en test", DateTime.Now.AddSeconds(10));
				await SettingsManager.SavePreferenceAsync(Constants.Notify, true);
			}
			else {
				await SettingsManager.SavePreferenceAsync(Constants.Notify, false);
				//TODO: clear all notifications (if needed)
			}
		}
    }

}
