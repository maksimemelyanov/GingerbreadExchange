using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using GingerbreadsExchange.Models;

namespace GingerbreadsExchange
{
    public class TaskManager
    {
        Repository repository = new Repository();
        public void ExecuteTheDeal(int id)
        {
            HostingEnvironment.QueueBackgroundWorkItem(x=>ExecuteAsync(id));
        }

        private async Task ExecuteAsync(int id)
        {
            History h = repository.History.FirstOrDefault(x => x.Id == id);
            h.SetStatus(Status.InProccess);
            repository.context.SaveChanges();
            Thread.Sleep(10000);
            h.SetStatus(Status.Completed);
            repository.context.SaveChanges();
        }  
    }
}