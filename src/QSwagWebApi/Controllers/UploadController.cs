#region Using

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QSwagGenerator.Annotations;

#endregion

namespace QSwagWebApi.Controllers
{
  [Route("api/package")]
  public class UploadController : Controller
  {
    #region Public

    /// <summary>
    ///   Package upload.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="files">The files.</param>
    /// <returns>Status of upload</returns>
    [HttpPost("upload")]
    [Tag("package")]
    public string GetPackagedDescriptor([FromForm] string id, [FromForm] IFormFileCollection files)
    {
      return "uploaded";
    }

    #endregion
  }
}