using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GingerbreadsExchange.Models
{
    public class Repository
    {

        public ExchangeContext context = new ExchangeContext();

        public IEnumerable<Buying> Buyings
        {
            get { return context.Buyings; }
        }

        public IEnumerable<Selling> Sellings
        {
            get { return context.Sellings; }
        }

        public IEnumerable<History> History
        {
            get { return context.History; }
        }
    }
}