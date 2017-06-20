using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GingerbreadsExchange.Models;

namespace GingerbreadsExchange.Controllers
{
    public class SellingController : ApiController
    {
        Repository repository = new Repository();
        // GET api/<controller>
        public IEnumerable<Selling> Get()
        {
            return repository.Sellings.Where(x => x.Count > 0);
        }

        // GET api/<controller>/5
        public Selling Get(int id)
        {
            return repository.Sellings.FirstOrDefault(x => x.Id == id);
        }

        // POST api/<controller>
        public void Post([FromBody]Selling selling)
        {
            selling = repository.context.Sellings.Add(selling);
            repository.context.SaveChanges();
            Dealer d = new Dealer();
            d.DealBySelling(selling.Id);

        }

    }
}