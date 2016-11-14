using System;
using System.Threading.Tasks;
using skolerute.data;
using skolerute.utils;
using skolerute.notifications;
using Xamarin.Forms;
using System.Collections.Generic;

namespace skolerute.Views
{
    public partial class SettingsPage : ContentPage
    {
		int daysBeforeNotification = 0;
		List<School> favoriteSchools = new List<School>();

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

			MessagingCenter.Subscribe<StartUpPage, School>(this, "choosenSch", (sender, args) =>
			{
				favoriteSchools.Add(args);
				DependencyService.Get<INotification>().RemoveCalendarNotification();
				if (notification.On)
				{
					NotificationUtils.SendNotifications(Calendar.AddSchoolToList(favoriteSchools), daysBeforeNotification);
				}
			});

			// Add days in picker (user chooses when to get notified)
			for (int i = 1; i < 31; i++)
			{
				listOfPickerDays.Items.Add(i.ToString());
			}

			//https://developer.xamarin.com/api/type/Xamarin.Forms.Picker/
			listOfPickerDays.SelectedIndexChanged += (sender, args) =>
				{
				if (listOfPickerDays.SelectedIndex == -1)
					{
						string DBUG = "e";
					}
					else
					{
					DependencyService.Get<INotification>().RemoveCalendarNotification();
					daysBeforeNotification = int.Parse(listOfPickerDays.Items[listOfPickerDays.SelectedIndex]);
					NotificationUtils.SendNotifications(Calendar.AddSchoolToList(favoriteSchools), daysBeforeNotification);
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
				NotificationUtils.SendNotifications(Calendar.AddSchoolToList(favoriteSchools), daysBeforeNotification);
				await SettingsManager.SavePreferenceAsync(Constants.Notify, true);
			}
			else {
				await SettingsManager.SavePreferenceAsync(Constants.Notify, false);
				DependencyService.Get<INotification>().RemoveCalendarNotification();
			}
		}
    }
}
