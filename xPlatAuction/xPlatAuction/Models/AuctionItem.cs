using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPlatAuction.Models
{
    public class AuctionItem : INotifyPropertyChanged
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double StartingBid { get; set; }

        private double bid;

        public double CurrentBid { 
            get { return bid; }
            set {
                if(value.CompareTo(bid) != 0){
                    bid = value;
                    if(PropertyChanged != null){
                        PropertyChanged(this, new PropertyChangedEventArgs("CurrentBid"));
                    }
                }
            } 
        }

        public string AuctionId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
