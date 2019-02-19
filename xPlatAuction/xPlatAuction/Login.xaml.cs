using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xPlatAuction.ViewModels;

namespace xPlatAuction
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            var model = new LoginViewModel(this.Navigation);
            this.BindingContext = model;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //initiate login attempt with cached credentials
            LoginViewModel model = this.BindingContext as LoginViewModel;
            model.TryLoginWithCachedCredentials();
        }
    }
}