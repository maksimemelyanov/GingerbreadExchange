using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace GingerbreadsExchange.Models
{
    public class ExchangeContext : DbContext
    {
        public DbSet<Buying> Buyings { get; set; }
        public DbSet<Selling> Sellings { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}