using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace skolerute.utils
{
    public static class SettingsManager
    {
        public static async Task SavePreferenceAsync(string key, object objectToSave)
        {
            Application.Current.Properties[key] = objectToSave;
            await Application.Current.SavePropertiesAsync();
        }

        public static object GetPreference(string key)
        {
            if (Application.Current.Properties.ContainsKey(key))
                return Application.Current.Properties[key];
            else
                return null;
        }
    }
}
