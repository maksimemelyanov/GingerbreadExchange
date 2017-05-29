using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GingerbreadsExchange.Models;
using System.Globalization;

namespace GingerbreadsExchange.Controllers
{
    public class HomeController : Controller
    {
        Repository rep = new Repository();
        public ActionResult Index()
        {

            ViewBag.Sellings = rep.Sellings.ToList().Where(t=>t.Count>0).OrderBy(t => t.Price);
            ViewBag.Buyings = rep.Buyings.ToList().Where(t => t.Count > 0).OrderBy(t => t.Price).Reverse();
            ViewBag.History = rep.History.ToList().OrderBy(t => t.Timestamp).Reverse();
            return View();
        }

        public ActionResult Buy()
        {
            NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            decimal p = decimal.Parse(Request.Form["price"], nfi);
            long c = long.Parse(Request.Form["count"]);
            string mail = Request.Form["mail"];
            Buying b = new Buying(p, c, mail);
            Buying saved_buying = rep.context.Buyings.Add(b);
            rep.context.SaveChanges();

            List<Selling> s = rep.Sellings.Where(t => (t.Count > 0 && t.Price <= saved_buying.Price)).OrderBy(t => t.Price).ToList();
            bool complete = false;
            int listSize = s.Count;
            History h;
            for (int i = 0; i < listSize && !complete; i++)
            {
                if (s[i].Count < saved_buying.Count)
                {
                    long count = s[i].Count;
                    saved_buying.CountUpdate(count);
                    s[i].CountUpdate(count);
                    h = new History(s[i], saved_buying, count);

                }
                    
                else
                {
                    long count = saved_buying.Count;
                    saved_buying.CountUpdate(count);
                    s[i].CountUpdate(count);
                    h = new History(s[i], saved_buying, count);
                    complete = true;

                }
                rep.context.History.Add(h);
                rep.context.SaveChanges();
            }
            return Redirect("/Home/Index");
        }

        public ActionResult Sell()
        {
            NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            decimal p = decimal.Parse(Request.Form["price"], nfi);
            long c = long.Parse(Request.Form["count"]);
            string mail = Request.Form["mail"];
            Selling s = new Selling(p, c, mail);
            Selling saved_selling = rep.context.Sellings.Add(s);
            rep.context.SaveChanges();

            List<Buying> b = rep.Buyings.Where(t => (t.Count > 0 && t.Price >= saved_selling.Price)).OrderBy(t => t.Price).Reverse().ToList();
            bool complete = false;
            int listSize = b.Count;
            History h;
            for (int i = 0; i < listSize && !complete; i++)
            {
                if (b[i].Count < saved_selling.Count)
                {
                    long count = b[i].Count;
                    saved_selling.CountUpdate(count);
                    b[i].CountUpdate(count);
                    h = new History(saved_selling, b[i], count);
                }

                else
                {
                    long count = saved_selling.Count;
                    saved_selling.CountUpdate(count);
                    b[i].CountUpdate(count);
                    h = new History(saved_selling, b[i], count);
                    complete = true;
                }
                rep.context.History.Add(h);
                rep.context.SaveChanges();
            }
            return Redirect("/Home/Index");
        }
    }
}