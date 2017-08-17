using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace QSwagWebApi.Controllers
{
    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    /// <summary>
    /// Credentials provider. With encrypted data at rest.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class UnderTest : Controller
    {
        /// <summary>
        /// Gets the credentials.
        /// </summary>
        /// <returns>Dictionary of credential names with masked values.</returns>
        [HttpGet, Route("credentials")]
        public Dictionary<string, string> GetCredentials()
        {
            return null;
        }
        /// <summary>
        /// Gets the credential by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Name value pair (value is masked)</returns>
        [HttpGet, Route("credentials/{name}")]
        public NameValue GetCredential(string name)
        { 
            return new NameValue() { Name = name };
        }
        /// <summary>
        /// Creates new credentials.
        /// </summary>
        /// <param name="nameValue">The credential key value pair.</param>
        /// <returns>Unencrypted credentials back.</returns>
        [HttpPost, Route("credentials")]
        public IActionResult CreateCredentials([FromBody] NameValue nameValue)
        {
           return (IActionResult)Ok(nameValue);
        }

        /// <summary>
        /// Updates the credentials.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="name">The name.</param>
        /// <returns>Microsoft.AspNetCore.Mvc.IActionResult.</returns>
        [HttpPut, Route("credentials/{name?}")]
        public IActionResult UpdateCredentials([FromBody] NameValue entity, [FromRoute] string name)
        {
            return (IActionResult)Ok(entity);
        }


        /// <summary>
        /// Deletes the credentials.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Microsoft.AspNetCore.Mvc.IActionResult.</returns>
        [HttpDelete, Route("credentials/{name}")]
        public IActionResult DeleteCredentials(string name)
        {
           return  (IActionResult)Ok();
        }
    }
}