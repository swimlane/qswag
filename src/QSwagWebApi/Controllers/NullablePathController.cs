#region Using

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QSwagGenerator.Annotations;
using QSwagWebApi.Models;

#endregion

namespace QSwagWebApi.Controllers
{
    [Route("api")]
    public class NullablePathController : Controller
    {
        /// <summary>
        /// Gets the application information.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <returns>application object</returns>
        [HttpGet("application/{appId?}")]
        public string GetApplication(string appId)
        {
            return appId;
        }

        /// <summary>
        /// Gets the application information.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <param name="recordId"></param>
        /// <returns>application object</returns>
        [HttpGet("application/{appId?}/record/{recordId?}")]
        public string GetApplicationRecord(string appId, string recordId)
        {
            return appId+recordId;
        }

        /// <summary>
        /// Gets the application information.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <param name="recordId"></param>
        /// <param name="fieldId"></param>
        /// <returns>application object</returns>
        [HttpGet("application/{appId?}/record/{recordId?}/field/{fieldId?}")]
        public string GetApplication(string appId, string recordId, string fieldId)
        {
            return appId+recordId+fieldId;
        }

        
    }
}