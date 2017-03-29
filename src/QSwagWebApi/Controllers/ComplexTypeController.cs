#region Using

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QSwagGenerator.Annotations;
using QSwagWebApi.Models;

#endregion

namespace QSwagWebApi.Controllers
{
    [Route("api/ComplexType")]
    public class ComplexTypeController : Controller
    {
        #region Access: Public

        /// <summary>
        /// Gets the person information.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <returns>Person object tm</returns>
        [HttpGet("person/{id}")]
        public Person GetPerson(int id)
        {
            return new Person();
        }

        [HttpGet("person")]
        [Tag("private", "test")]
        public IEnumerable<Person> GetPersons()
        {
            return new[] {new Person()};
        }

        [HttpPost("person")]
        public void PostPerson(Person person, string id, int children, string[] hobbies, bool single)
        {
        }

        [HttpPut("person/{id}")]
        public void PostPersons(string id, IEnumerable<Person> person)
        {
        }

        #endregion
    }
}