using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GingerbreadsExchange.Models
{
    public class Selling
    {
        public int Id { get; protected set; }
        public DateTime Timestamp { get; protected set; }
        public Decimal Price { get; protected set; }
        public long Count { get; protected set; }
        public string Mail { get; protected set; }

        public Selling(Decimal price, long count, string mail = "")
        {
            Timestamp = DateTime.Now;
            Price = price;
            Count = count;
            Mail = mail;
        }

        public bool CountUpdate(long c)
        {
            bool result = true;
            if (c <= this.Count)
                Count -= c;
            else
                result = false;
            return result;
        }
        public Selling()
        {

        }
    }
}