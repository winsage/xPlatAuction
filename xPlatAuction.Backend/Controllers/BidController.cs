using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using xPlatAuction.Backend.DataObjects;
using xPlatAuction.Backend.Models;
using System.Security.Claims;
using System;

namespace xPlatAuction.Backend.Controllers
{
    [Authorize]
    public class BidController : TableController<Bid>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Bid>(context, Request);
        }

        // GET tables/Bid
        public IQueryable<Bid> GetAllBid()
        {
            return Query(); 
        }

        // GET tables/Bid/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Bid> GetBid(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Bid/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Bid> PatchBid(string id, Delta<Bid> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Bid
        public async Task<IHttpActionResult> PostBid(Bid item)
        {
            //get the logged in user information and extract name identifier
            var user = this.User as ClaimsPrincipal;
            if (user != null)
            {
                var userIDClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIDClaim != null)
                {
                    item.Bidder = userIDClaim.Value;
                }
            }

            Bid current = await InsertAsync(item);

            //send a notification about the new bid
            var msg = new TemplatePushMessage();
            msg.Add("ItemName", current.AuctionItem.Name);
            msg.Add("BidAmount", item.BidAmount.ToString());

            try
            {
                var outcome = await this.Configuration.GetPushClient().SendAsync(msg);
            }
            catch(Exception ex)
            {
                Configuration.Services.GetTraceWriter().Error("Could not send notifiation message", ex);
            }

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Bid/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteBid(string id)
        {
             return DeleteAsync(id);
        }
    }
}
