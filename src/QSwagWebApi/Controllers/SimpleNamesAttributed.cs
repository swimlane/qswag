using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace QSwagWebApi.Controllers
{
    [Route("api/SimpleNamesAttributed")]
    public class SimpleNamesAttributed : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id )
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet, Route("/swagger/SimpleNamesAttributed")]
        public string GetSwagger()
        {
            QSwagGenerator.WebApiToSwagger.GenerateForController(GetType(),new QSwagGenerator.GeneratorSettings(),nameof(GetSwagger));
            throw new NotImplementedException();
        }
    }
}
