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
    public partial class AuctionItems
    {
        public AuctionItems()
        {
            InitializeComponent();
        }
        public AuctionItems(Auction auction) : this()
        {
            this.BindingContext = new AuctionItemsViewModel(auction, Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((AuctionItemsViewModel)this.BindingContext).Load();
        }

   /*     protected void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(
                new PlaceBid((AuctionItem)e.Item));    
        }
        */
    }
}
