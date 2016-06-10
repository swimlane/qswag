using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace QSwagWebApi.Controllers
{
    /// <summary>
    /// Different types of routes controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class UnderTestController : Controller
    {
        /// <summary>
        /// Applications the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Map of values</returns>
        [HttpGet, Route("[action]/{id?}")]
        // ReSharper disable once InconsistentNaming
        public Dictionary<string,string> GetDictionary(string id)
        {
            return new Dictionary<string, string> { {id,"34"} };
        }      
        /// <summary>
        /// Applications the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Map of values</returns>
        [HttpGet, Route("[action]")]
        // ReSharper disable once InconsistentNaming
        public IEnumerable<string> GetList(string id)
        {
            return new List<string>  {id,"34"};
        }
    }
}
