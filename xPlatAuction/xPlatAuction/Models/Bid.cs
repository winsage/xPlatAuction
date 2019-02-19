using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPlatAuction.Models
{
    public class Bid
    {
        public string Id { get; set; }
        public string AuctionItemId { get; set; }
        public double BidAmount { get; set; }
    }
}
