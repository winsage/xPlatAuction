using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xPlatAuction.Models;

namespace xPlatAuction.ViewModels
{
    public class LoginViewModel : ViewModelBase, INotifyPropertyChanged
    {
      

        public LoginViewModel(INavigation nav):base(nav)
        {
            LoginCommand = new Command(Login);
            MessagingCenter.Subscribe<AuctionService, LoginResult>(this, Constants.MSG_LOGIN_COMPLETE, LoginComplete);
        }

        private string _loginMsg = "Attempting to login with cached credentials";

        public string LoginMessage
        {
            get { return _loginMsg; }
            set
            {
                if (value != _loginMsg)
                {
                    base.NotifyPropertyChanged("LoginMessage");
                }
                _loginMsg = value;
            }
        }

        public ICommand LoginCommand
        {
            get;
            private set;
        }

        public void Login()
        {
            App.GetAuctionService().Login();
        }

        public async Task TryLoginWithCachedCredentials()
        {
             await App.GetAuctionService().TryLoginWithCachedCredentialsAsync();
        }
        public void LoginComplete(AuctionService svc, LoginResult result)
        {
            //was the login successful
            if(!result.Succeeded)
            {
                //they couldn't log in, so we'll show the message
                LoginMessage = result.ErrorMessage;
            }
            else
            {
                //unsubscribe from messages
                MessagingCenter.Unsubscribe<AuctionService>(this, Constants.MSG_LOGIN_COMPLETE);    
            }

        }
    }
}
