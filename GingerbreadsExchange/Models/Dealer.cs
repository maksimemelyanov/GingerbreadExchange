using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GingerbreadsExchange.Models
{
    public class Dealer
    {
        Repository rep = new Repository();
        public void DealByBuying(int id)
        {
            Buying buying = rep.Buyings.FirstOrDefault(t => t.Id == id);
            if (buying != null)
            {
                List<Selling> s = rep.Sellings.Where(t => (t.Count > 0 && t.Price <= buying.Price)).OrderBy(t => t.Price).ToList();
                bool complete = false;
                int listSize = s.Count;
                History h;
                for (int i = 0; i < listSize && !complete; i++)
                {
                    if (s[i].Count < buying.Count)
                    {
                        long count = s[i].Count;
                        buying.CountUpdate(count);
                        rep.context.SaveChanges();
                        s[i].CountUpdate(count);
                        rep.context.SaveChanges();
                        h = new History(s[i], buying, count);

                    }

                    else
                    {
                        long count = buying.Count;
                        buying.CountUpdate(count);
                        rep.context.SaveChanges();
                        s[i].CountUpdate(count);
                        rep.context.SaveChanges();
                        h = new History(s[i], buying, count);
                        complete = true;

                    }
                    rep.context.History.Add(h);
                    rep.context.SaveChanges();
                }
            }
        }

        public void DealBySelling(int id)
        {
            Selling selling = rep.Sellings.FirstOrDefault(t => t.Id == id);
            if (selling != null)
            {
                List<Buying> b = rep.Buyings.Where(t => (t.Count > 0 && t.Price >= selling.Price)).OrderBy(t => t.Price).Reverse().ToList();
                bool complete = false;
                int listSize = b.Count;
                History h;
                for (int i = 0; i < listSize && !complete; i++)
                {
                    if (b[i].Count < selling.Count)
                    {
                        long count = b[i].Count;
                        selling.CountUpdate(count);
                        rep.context.SaveChanges();
                        b[i].CountUpdate(count);
                        rep.context.SaveChanges();

                        h = new History(selling, b[i], count);
                    }

                    else
                    {
                        long count = selling.Count;
                        selling.CountUpdate(count);
                        rep.context.SaveChanges();
                        b[i].CountUpdate(count);
                        rep.context.SaveChanges();
                        h = new History(selling, b[i], count);
                        complete = true;
                    }
                    rep.context.History.Add(h);
                    rep.context.SaveChanges();
                }
            }
        }
    }
}