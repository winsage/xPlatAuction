using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using xPlatAuction.Models;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace xPlatAuction.ViewModels
{
    public class AuctionsViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public AuctionsViewModel(INavigation navigation) :base(navigation)
        {
            SelectAuction = new Command<Auction>(GoToAuction);
            PlaceBid = new Command<MyAuctionItem>(BidOnMyItem);
            RefreshMyItems = new Command(LoadMyItems);
            RefreshAuctions = new Command(LoadAuctions);

            MessagingCenter.Subscribe<PlaceBidViewModel, AuctionItem>(this, Constants.MSG_ITEMUPDATED, ItemUpdated);
        }

       //command and delegate to move to auction items
        public ICommand SelectAuction
        {
            get;
            private set;
        }

        public void GoToAuction(Auction auction)
        {
			Navigation.PushAsync(
				new AuctionItems(auction));
        }

        // command and delegate to move to place bid
        public ICommand PlaceBid
        {
            get;private set;

        }

        public void BidOnMyItem(MyAuctionItem item)
        {
			var targetItem = new AuctionItem
			{
				Id = item.Id,
				Name = item.Name,
				Description = item.Description,
				CurrentBid = item.CurrentBid
			};

			Navigation.PushAsync(new PlaceBid(targetItem));
        }

        // command and delegate to load auctions
        public ICommand RefreshAuctions
        {
            get;private set;
        }

        public async void LoadAuctions(object parameter)
        {
			IsLoading = true;
            try{
                var auctionResults = await App.GetAuctionService().GetAuctions();
                Auctions = new ObservableCollection<Auction>(auctionResults);
            }
            catch(Exception ex)
            {
                //TODO: alert to error
            }
            finally
            {
                IsLoading = false;
            }
        }

        // command and delegate to load custom items
        public ICommand RefreshMyItems
        {
            get;private set;
        }

        public async void LoadMyItems(object parameter)
        {
            IsLoading = true;
            try{
                var itemsResult = await App.GetAuctionService().GetMyItems();
                MyAuctionItems = new ObservableCollection<MyAuctionItem>(itemsResult);
            }
			catch(Exception ex)
            {
                //TODO: alert to error
            }
            finally 
            {
                IsLoading = false;
            }
        }

        // collections for auctions and items

        private ObservableCollection<MyAuctionItem> myItems;

        public ObservableCollection<MyAuctionItem> MyAuctionItems
        {
            get { return myItems; }
            set { 
                myItems = value;
                NotifyPropertyChanged("MyAuctionItems");
            }
        }

        private ObservableCollection<Auction> auctionList;

        public ObservableCollection<Auction> Auctions {
            get { return auctionList; }
            set
            {
                auctionList = value;
                NotifyPropertyChanged("Auctions");
            }
        }

        //trigger to load all items
        public void Load()
        {
            //escape if already loaded
            if (Auctions != null)
                return;

             LoadAuctions(null);
             LoadMyItems(null);

        }

		public void ItemUpdated(PlaceBidViewModel model, AuctionItem item)
		{
            if (MyAuctionItems != null)
			{
                var targetItem = MyAuctionItems.FirstOrDefault((i) => i.Id == item.Id);
                if(targetItem != null) 
                {
                    targetItem.CurrentBid = item.CurrentBid;
                }
				
			}
		}

    }
}
