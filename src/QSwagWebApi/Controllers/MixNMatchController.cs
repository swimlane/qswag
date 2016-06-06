using Microsoft.AspNetCore.Mvc;

namespace QSwagWebApi.Controllers
{
    /// <summary>
    /// Different types of routes controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class MixNMatchController : Controller
    {
        /// <summary>
        /// Gets the value by identifier.
        /// </summary>
        /// <param name="id">The value pair id.</param>
        /// <returns>The value</returns>
        [HttpGet, Route("/valuespair/{id}")]
        public string GetValueById(int id)
        {
            return "29";
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Default value</returns>
        [HttpGet, Route("default/{id?}")]
        public string GetDefaultValue(string id)
        {
            return string.IsNullOrEmpty(id) ? "29" : id;
        }

        /// <summary>
        /// Gets the record per application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet, Route("app/{app?}/record/{id}")]
        public string GetRecordPerApp(string app, string id)
        {
            return string.IsNullOrEmpty(id) ? "29" : id;
        }

        /// <summary>
        /// Applications the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [HttpGet, Route("[action]/{id?}")]
        // ReSharper disable once InconsistentNaming
        public string application(string id, string query)
        {
            return string.IsNullOrEmpty(id) ? "29" : id;
        }
    }
}
