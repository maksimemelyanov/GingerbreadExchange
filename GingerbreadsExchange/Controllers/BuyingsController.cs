using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GingerbreadsExchange.Models;

namespace GingerbreadsExchange.Controllers
{
    public class BuyingsController : ApiController
    {
        Repository repository = new Repository();
        // GET api/<controller>
        public IEnumerable<Buying> Get()
        {
            return repository.Buyings.Where(x => x.Count > 0);
        }

        // GET api/<controller>/5
        public Buying Get(int id)
        {
            return repository.Buyings.FirstOrDefault(x => x.Id == id);
        }

        // POST api/<controller>
        public void Post([FromBody]Buying buying)
        {
            buying = repository.context.Buyings.Add(buying);
            repository.context.SaveChanges();
            Dealer d = new Dealer();
            d.DealByBuying(buying.Id);

        }


    }
}