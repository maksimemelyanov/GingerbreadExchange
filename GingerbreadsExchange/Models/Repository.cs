using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GingerbreadsExchange.Models
{
    public class Repository : IDisposable
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

        public IEnumerable<Currency> Currencies
        {
            get { return context.Currencies; }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}