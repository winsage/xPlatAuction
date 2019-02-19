using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace xPlatAuction.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private INavigation nav;
        
        public ViewModelBase(INavigation navigation)
        {
            nav = navigation;
        }

        
        private bool loading;

        public bool IsLoading
        {
            get { return loading; }
            set
            {
                loading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get { return nav; } }
    }
}
