using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MobileMonitor.Controllers
{
    public class PushController : ApiController
    {
        // GET api/<controller>
        [Route("api/Push")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [Route("api/Push/{id}")]
        public string Get(int id)
        {

            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
            string notification = value;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}