#region Using

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace QSwagWebApi.Controllers
{
    [Route("api/splitcontroller")]
    public class SplitTwoController: Controller
    {
        #region Access: Public

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        
        // PUT api/values/5
        [HttpPatch("{id}")]
        public void Patch(int id, [FromBody] string value)
        {
        }

        #endregion
    }
}