using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using xPlatAuction.Backend.Models;
using System.Collections.Generic;
using xPlatAuction.Backend.DataObjects;
using System.Linq;
using System.Security.Claims;

namespace xPlatAuction.Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class MyItemsController : ApiController
    {
        public IEnumerable<MyAuctionItem> Get()
        {
            string userId = string.Empty;

            var user = this.User as ClaimsPrincipal;
            if(user!=null)
            {
                var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if(userIdClaim != null)
                {
                    userId = userIdClaim.Value;
                }
            }

            MobileServiceContext ctx = new MobileServiceContext();
            var myItems = from ai in ctx.AuctionItems
                          join bid in ctx.Bids on  ai.Id equals bid.AuctionItemId
                          where bid.Bidder == userId
                          select new MyAuctionItem
                          {
                              Id = ai.Id,
                              Name = ai.Name,
                              Description = ai.Description,
                              CurrentBid = ai.Bids.Count == 0 ? 0 : ai.Bids.Max(b => b.BidAmount),
                              MyHighestBid = ai.Bids.Where(
                                  b=>b.Bidder == userId).Max(
                                    b=>b.BidAmount)
                          };

            return myItems;
        }
    }
}
