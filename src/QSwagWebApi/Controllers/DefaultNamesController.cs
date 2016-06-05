using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace QSwagWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DefaultNamesController : Controller
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("{id}")]
        public string Get(int id )
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [Route("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [Route("{id}")]
        public void Delete(int id)
        {
        }
    }
}
