using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xPlatAuction.Models;

namespace xPlatAuction.ViewModels
{
    public class PlaceBidViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private AuctionItem targetItem;

        public PlaceBidViewModel(AuctionItem item, INavigation navigation) : base(navigation)
        {
            targetItem = item;
			BidAmount = item.CurrentBid == 0 ? item.StartingBid : item.CurrentBid + 5;
            PlaceBidCommand = new Command<Bid>(ExecutePlaceBid, CanExecutePlaceBid);
        }
        

        private double amount;
        public double BidAmount { 
            get{return amount;}
            set {
                if (amount != value)
                {
                    amount = value;
                    NotifyPropertyChanged("BidAmount");
                }
            } 
        }


        public AuctionItem Item
        {
            get { return targetItem; }
            set
            {
                targetItem = value;
                NotifyPropertyChanged("Item");
            }
        }

        public ICommand PlaceBidCommand { get; private set; }

        public async void ExecutePlaceBid(Bid parameter)
        {
            try
            {
                var newBid = await App.GetAuctionService().PlaceBid(
                    new Bid { AuctionItemId = targetItem.Id, BidAmount = this.BidAmount });

                Item.CurrentBid = newBid.BidAmount;

                MessagingCenter.Send<PlaceBidViewModel, AuctionItem>(this, Constants.MSG_ITEMUPDATED, Item);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            //move back to the page they were on before bidding
            await Navigation.PopAsync();

        }
        public bool CanExecutePlaceBid(Bid parameter)
        {
            return Item.CurrentBid < BidAmount;
        }
    }
}
