using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GingerbreadsExchange.Models;

namespace GingerbreadsExchange.Controllers
{
    public class AdminController : Controller
    {
        Repository repository = new Repository();
        // GET: Admin
        public ActionResult Index()
        {
            var history = repository.History.Where(t => t.Status != Status.Completed).OrderBy(t=>t.Timestamp).ToList();
            return View(history);
        }

        public ActionResult Verify(int id=0)
        {
            History h = repository.History.FirstOrDefault(t => t.Id == id);
            if (h == null)
                return HttpNotFound();
            h.SetStatus(Status.Verified);
            repository.context.SaveChanges();
            TaskManager tm = new TaskManager();
            tm.ExecuteTheDeal(h.Id);
            return RedirectToAction("Index");
        }

        public ActionResult Reject(int id = 0)
        {
            History h = repository.History.FirstOrDefault(t => t.Id == id);
            if (h == null)
                return HttpNotFound();
            h.Selling.CountUpdate(-h.Count);
            repository.context.SaveChanges();
            h.Buying.CountUpdate(-h.Count);
            repository.context.SaveChanges();
            repository.context.History.Remove(h);
            repository.context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}