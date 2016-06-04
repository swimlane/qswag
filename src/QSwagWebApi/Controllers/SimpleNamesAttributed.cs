using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QSwagWebApi.Models;

namespace QSwagWebApi.Controllers
{
    [Route("api/SimpleNamesAttributed")]
    public class SimpleNamesAttributedController : Controller
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

        [HttpGet("person/{id}")]
        public Person GetPerson(int id)
        {
            return new Person();
        }
        [HttpPost("person")]
        public void PostPerson(Person person)
        {
        }

        [HttpGet, Route("/swagger/SimpleNamesAttributed")]
        public string GetSwagger()
        {
            var generatorSettings = new QSwagGenerator.GeneratorSettings() {
                DefaultUrlTemplate = "api/[controller]/{id?}",
                IgnoreObsolete =true };
          return  QSwagGenerator.WebApiToSwagger.GenerateForController(GetType(),generatorSettings,nameof(GetSwagger));
        }
    }
}
