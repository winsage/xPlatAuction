using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xPlatAuction.ViewModels;
using xPlatAuction.Models;

namespace xPlatAuction
{
    public partial class PlaceBid
    {
        public PlaceBid(AuctionItem item)
        {
            InitializeComponent();
            this.BindingContext = new PlaceBidViewModel(item, Navigation);
        }
    }
}
