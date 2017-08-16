using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QSwagGenerator.Annotations;

namespace QSwagWebApi.Controllers
{
    /// <summary>
    /// Different types of routes controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class UnderTestController : Controller
    {
        /// <summary>
        /// Imports application (and surrounding entities).
        /// </summary>
        /// <param name="id">The root application identifier.</param>
        /// <param name="json">The import json.</param>
        /// <returns>
        /// Collection of imprted applications: id and name.
        /// </returns>
        [Tag("application")]
        [HttpPost, Route("app/{id?}/import")]
        public void ApplicationImport(string id, [FromBody] dynamic json)
        {
            throw new NotImplementedException();
        }
    }
}
