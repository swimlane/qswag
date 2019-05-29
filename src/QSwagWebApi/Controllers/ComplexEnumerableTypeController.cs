#region Using

using Microsoft.AspNetCore.Mvc;
using QSwagWebApi.Models;

#endregion

namespace QSwagWebApi.Controllers
{
    [Route("api/ComplexListType")]
    public class ComplexEnumerableTypeController : Controller
    {
        #region Access: Public
        
        [HttpGet("packageddescriptor/{id}")]
        public PackagedDescriptor GetPackagedDescriptor(int id)
        {
            return new PackagedDescriptor();
        }

        #endregion
    }
}
