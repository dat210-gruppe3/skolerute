using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skolerute.utils;
using skolerute.data;

using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            // Checking to see if the value of offline mode has been set by the user
            if(SettingsManager.GetPreference(Constants.offlineMode) != null)
            {
                offlineMode.SetValue(SwitchCell.OnProperty, SettingsManager.GetPreference(Constants.offlineMode));
            }
        }

        async Task OnOfflineModeChanged(object sender, EventArgs ea)
        {
            //TODO: Check to see whether or not offline mode has been set in the past, if not set it to false when the
            // app starts
            if (offlineMode.On == true)
            {
                await SettingsManager.SavePreferenceAsync(Constants.offlineMode, true);
                //TODO: Start parsing all data from csv file into database
            }
            else if(!offlineMode.On)
            {
                await SettingsManager.SavePreferenceAsync(Constants.offlineMode, false);
            }
        }
    }

}
