using Microsoft.Azure.Mobile.Server;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using xPlatAuction.Backend.DataObjects;


namespace xPlatAuction.Backend.Models
{
    public class AuctionItemDomainManager : MappedEntityDomainManager<AuctionItem, AuctionItemDBEntity>
    {
        public AuctionItemDomainManager(DbContext context, 
            System.Net.Http.HttpRequestMessage request): base(context, request)
        { }

        public override IQueryable<AuctionItem> Query()
        {
            MobileServiceContext ctx = this.Context as MobileServiceContext;

            var items = from ai in ctx.AuctionItems
                        select new AuctionItem
                        {
                            Id = ai.Id,
                            Description = ai.Description,
                            StartingBid = ai.StartingBid,
                            Name = ai.Name,
                            AuctionId = ai.AuctionId,
                            Deleted = ai.Deleted,
                            CreatedAt = ai.CreatedAt,
                            UpdatedAt = ai.UpdatedAt,
                            Version = ai.Version,
                            CurrentBid = ai.Bids.Count == 0 ? 0 : ai.Bids.Max(b => b.BidAmount)
                        };
            return items;
        }
       
        public override System.Web.Http.SingleResult<AuctionItem> Lookup(string id)
        {

            MobileServiceContext ctx = this.Context as MobileServiceContext;

            var item = from ai in ctx.AuctionItems
                       where ai.Id == id
                       select new AuctionItem
                       {
                           Id = ai.Id,
                           Description = ai.Description,
                           StartingBid = ai.StartingBid,
                           Name = ai.Name,
                           AuctionId = ai.AuctionId,
                           Deleted = ai.Deleted,
                           CreatedAt = ai.CreatedAt,
                           UpdatedAt = ai.UpdatedAt,
                           Version = ai.Version,
                           CurrentBid = ai.Bids.Count == 0 ? 0 : ai.Bids.Max(b => b.BidAmount)
                       };
            return new System.Web.Http.SingleResult<AuctionItem>(item);

        }

        public override Task<bool> DeleteAsync(string id)
        {
            return base.DeleteItemAsync(id);
        }
        public override Task<AuctionItem> UpdateAsync(string id, System.Web.Http.OData.Delta<AuctionItem> patch)
        {
            return base.UpdateEntityAsync(patch, id);
        }

       
    }
}
