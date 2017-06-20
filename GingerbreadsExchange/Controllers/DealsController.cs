using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GingerbreadsExchange.Models;

namespace GingerbreadsExchange
{
    public class DealsController : ApiController
    {
        Repository repository = new Repository();
        // GET api/<controller>
        public IEnumerable<History> GetAll()
        {
            return repository.History;
        }

        // GET api/<controller>/5
        public History Get(int id)
        {
            return repository.History.FirstOrDefault(x => x.Id == id);
        }


    }
}