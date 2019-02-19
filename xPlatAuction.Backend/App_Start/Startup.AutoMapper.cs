using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using xPlatAuction.Backend.DataObjects;
using xPlatAuction.Backend.Models;

namespace xPlatAuction.Backend
{
    public partial class Startup
    {
        public static void ConfigureAutoMapper(HttpConfiguration config)
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AuctionItem, AuctionItemDBEntity>();
                cfg.CreateMap<AuctionItemDBEntity, AuctionItem>().ForMember(
                    ai => ai.CurrentBid, map => map.UseValue(0.0));
            });
        }
    }
}