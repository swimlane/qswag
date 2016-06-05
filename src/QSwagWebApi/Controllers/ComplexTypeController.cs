#region Using

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QSwagWebApi.Models;

#endregion

namespace QSwagWebApi.Controllers
{
    [Route("api/ComplexType")]
    public class ComplexTypeController : Controller
    {
        #region Access: Public

        [HttpGet("person/{id}")]
        public Person GetPerson(int id)
        {
            return new Person();
        }

        [HttpGet("person")]
        public IEnumerable<Person> GetPersons()
        {
            return new[] {new Person()};
        }

        [HttpPost("person")]
        public void PostPerson(Person person, string id, int children, string[] hobbies, bool single)
        {
        }

        [HttpPost("person")]
        public void PostPersons(IEnumerable<Person> person)
        {
        }

        #endregion
    }
}