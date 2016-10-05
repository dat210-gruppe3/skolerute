using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();

            // Placeholder liste over favoritt-skoler
            List<string> favorites = new List<string> { "skole1", "skole2", "skole3" };
            SchoolPicker.ItemsSource = favorites;

        }
    }
}
