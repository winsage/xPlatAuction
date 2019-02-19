using Microsoft.Azure.Mobile.Server;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using xPlatAuction.Backend.DataObjects;

namespace xPlatAuction.Backend.Models
{
    public class AuctionItemDBEntity : EntityData
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double StartingBid { get; set; }


        //EF properties
        [Column("Auction_Id")]
        public string AuctionId { get; set; }

        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }

        public virtual Collection<Bid> Bids { get; set; }
    }
}
