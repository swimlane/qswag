using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace QSwagWebApi.Controllers
{
    /// <summary>
    /// Unknown provider. 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class  DynamicController: Controller
    {
        /// <summary>
        /// Gets the credentials.
        /// </summary>
        /// <returns>Dictionary of credential names with masked values.</returns>
        [HttpGet, Route("unknown")]
        public dynamic GetUnknown()
        {
            return null;
        }
        /// <summary>
        /// Gets the credential by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Name value pair (value is masked)</returns>
        [HttpPost, Route("unknown/{name}")]
        public void PostUnknown(dynamic name)
        { 
        }
        /// <summary>
        /// Creates new credentials.
        /// </summary>
        /// <param name="nameValue">The credential key value pair.</param>
        /// <returns>Unencrypted credentials back.</returns>
        [HttpPut("unknown/{name}")]
        public IActionResult PutUnknown([FromBody] dynamic nameValue)
        {
           return (IActionResult)Ok(nameValue);
        }
        
        /// <summary>
        /// Patches credentials.
        /// </summary>
        /// <param name="nameValue">The credential key value pair.</param>
        /// <returns>Unencrypted credentials back.</returns>
        [HttpPatch("unknown/{name}")]
        public IActionResult PatchUnknown([FromBody] dynamic nameValue)
        {
            return (IActionResult)Ok(nameValue);
        }
    }
}