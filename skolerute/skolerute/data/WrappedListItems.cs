using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolerute.data
{
    public class WrappedListItems<T> : INotifyPropertyChanged
    {
        public T Item { get; set; }
        bool isChecked = false;
        public double Distance { get; set; }
        public bool DistanceVisible { get; set; }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }
            }
        }
        private bool unChecked = true;
        public bool UnChecked
        {
            get
            {
                return unChecked;
            }
            set
            {
                if (unChecked != value)
                {
                    unChecked = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("UnChecked"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
