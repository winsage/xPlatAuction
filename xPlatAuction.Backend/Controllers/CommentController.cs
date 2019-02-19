using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using xPlatAuction.Backend.DataObjects;
using xPlatAuction.Backend.Models;
using System.Collections.Generic;
using System.Web.Http.OData.Query;

namespace xPlatAuction.Backend.Controllers
{
    [Authorize]
    public class CommentController : TableController<Comment>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            DomainManager = new StorageDomainManager<Comment>("MS_StorageConnectionString", "comments", Request);
        }

        // GET tables/Comment
        public Task<IEnumerable<Comment>> GetAllComment(ODataQueryOptions options)
        {
            return base.QueryAsync(options); 
        }

        // GET tables/Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<SingleResult<Comment>> GetComment(string id)
        {
            return LookupAsync(id);
        }

        // PATCH tables/Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Comment> PatchComment(string id, Delta<Comment> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Comment
        public async Task<IHttpActionResult> PostComment(Comment item)
        {
            Comment current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteComment(string id)
        {
             return DeleteAsync(id);
        }
    }
}
