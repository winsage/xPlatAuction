using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using xPlatAuction.ViewModels;
using Xamarin.Forms;
using xPlatAuction.Models;

namespace xPlatAuction
{
    public partial class Auctions
    {
        public Auctions()
        {
            InitializeComponent();
            this.BindingContext = new AuctionsViewModel(this.Navigation);
            //ToolbarItems.Clear();
        }

        protected override void OnAppearing()
        {
           base.OnAppearing();
           ((AuctionsViewModel)BindingContext).Load();
        }

    }
}
