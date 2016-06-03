using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace QSwagWebApi.Controllers
{
    [Route("api/[controller]")]
    public class MixNMatchController : Controller
    {
       [HttpGet, Route("/valuespair/{id}")]
        public string GetValueById(int id)
        {
            return "29";
        }

        [HttpGet, Route("default/{id?}")]
        public string GetDefaultValue(string id)
        {
            return string.IsNullOrEmpty(id) ? "29" : id;
        }

        [HttpGet, Route("app/{app?}/record/{id}")]
        public string GetRecordPerApp(string app, string id)
        {
            return string.IsNullOrEmpty(id) ? "29" : id;
        }

        [HttpGet, Route("[action]/{id?}")]
        // ReSharper disable once InconsistentNaming
        public string application(string id, string query)
        {
            return string.IsNullOrEmpty(id) ? "29" : id;
        }
    }
}
