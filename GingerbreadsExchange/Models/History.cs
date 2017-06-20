using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace GingerbreadsExchange.Models
{
    [Table("History")]
    public class History
    {
        public int Id { get; protected set; }
        public DateTime Timestamp { get; protected set; }
        public virtual Selling Selling { get; protected set; }
        public virtual Buying Buying { get; protected set; }
        public long Count { get; protected set; }
        public Status Status { get; protected set; }
        public History(Selling s, Buying b, long c)
        {
            Timestamp = DateTime.Now;
            Selling = s;
            Buying = b;
            Count = c;
        }

        public History()
        {

        }

        public void SetStatus (Status s)
        {
            Status = s;
        }

    }
}