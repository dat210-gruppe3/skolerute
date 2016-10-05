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
			var label = new Label { Text = "This is a label.", TextColor = Color.FromHex("#77d065"), FontSize = 20 };
			var label2 = new Label { Text = "This is a222 222label.", TextColor = Color.FromHex("#77d065"), FontSize = 20 };

			layout.Children.Add(label);
			layout.Children.Add(label2);

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
