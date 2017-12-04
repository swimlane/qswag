#region Using

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace QSwagWebApi.Controllers
{
    [Route("api/splitcontroller")]
    public class SplitControllerOne : Controller
    {
        #region Access: Public

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new [] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        #endregion
    }
}