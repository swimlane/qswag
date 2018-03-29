#region Using

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace QSwagWebApi.Controllers
{
    [Route("api/splitcontroller")]
    public class SplitOneController : Controller
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
        public string GetById(int id)
        {
            return "value";
        }
        #endregion
    }
}