using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

        }

        void varseltoggler(object sender, EventArgs ea)
        {
            if (varseltoggle.On == true)
            {
                varseltoggle.Text = "Varsling på";
            }
            else varseltoggle.Text = "Varsling av";
        }

    }

}
