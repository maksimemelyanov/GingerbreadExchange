using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GingerbreadsExchange.Models;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;

namespace GingerbreadsExchange.Controllers
{
    public class HomeController : Controller
    {
        decimal ratio = 1;
        Repository rep = new Repository();
        public ActionResult Index()
        {
            ViewBag.Sellings = rep.Sellings.ToList().Where(t => t.Count > 0).OrderBy(t => t.Price);
            ViewBag.Buyings = rep.Buyings.ToList().Where(t => t.Count > 0).OrderBy(t => t.Price).Reverse();
            ViewBag.History = rep.History.Where(t => t.Status == Status.Completed).ToList().OrderBy(t => t.Timestamp).Reverse();
            ViewBag.Currencies = rep.Currencies.ToList();
            LoadCurrency();
            ViewBag.Ratio = ratio;
            return View();
        }

        private void LoadCurrency()
        {
            string cur = "";
            if (Request.Cookies["currency"] != null)
                cur = Request.Cookies["currency"].Value;
            else
                cur = "RUR";
            Currency c = rep.Currencies.First(x => x.Name == cur);
            ratio = c.Value;
        }

        public ActionResult Buy()
        {
            LoadCurrency();
            NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            decimal p = decimal.Parse(Request.Form["price"], nfi);
            long c = long.Parse(Request.Form["count"]);
            string mail = Request.Form["mail"];
            Buying b = new Buying(p*ratio, c, mail);
            Buying saved_buying = rep.context.Buyings.Add(b);
            rep.context.SaveChanges();

            Dealer d = new Dealer();
            d.DealByBuying(saved_buying.Id);
            return Redirect("/Home/Index");
        }

        public ActionResult Sell()
        {
            LoadCurrency();
            NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            decimal p = decimal.Parse(Request.Form["price"], nfi);
            long c = long.Parse(Request.Form["count"]);
            string mail = Request.Form["mail"];
            Selling s = new Selling(p*ratio, c, mail);
            Selling saved_selling = rep.context.Sellings.Add(s);
            rep.context.SaveChanges();

            Dealer d = new Dealer();
            d.DealBySelling(saved_selling.Id);
            return Redirect("/Home/Index");
        }

        public ActionResult ChangeCulture(string currency)
        {
            UpdateCurrencies();
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            Currency c = rep.Currencies.FirstOrDefault(x => x.Name == currency);
            if (c == null)
            {
                c = rep.Currencies.First();
            }
            HttpCookie cookie = Request.Cookies["currency"];
            if (cookie != null)
                cookie.Value = c.Name;
            else
            {
                cookie = new HttpCookie("currency");
                cookie.HttpOnly = false;
                cookie.Value = c.Name;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            ratio = c.Value;
            return Redirect(returnUrl);
        }

        public void UpdateCurrencies()
        {
            CBReader cb = new CBReader();
            List<Currency> currencies = cb.GetExchangeRates();
            foreach (var item in currencies)
            {
                Currency c = rep.Currencies.FirstOrDefault(x => x.Name == item.Name);
                if (c == null)
                {
                    c = new Currency(item.Name, item.Value);
                    rep.context.Currencies.Add(c);
                    rep.context.SaveChanges();
                }
                else
                {
                    c.ChangeValue(item.Value);
                    rep.context.SaveChanges();
                }

            }
        }
    }


}