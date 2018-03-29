#region Using

using Microsoft.AspNetCore.Mvc;

#endregion

namespace QSwagWebApi.Controllers
{
  /// <summary>
  ///   Wrong method name controller.
  /// </summary>
  /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
  [Route("api/splitcontroller")]
  public class SplitThreeController : Controller
  {
    #region Public

    /// <summary>
    ///   Gets the light version.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}/light")]
    public string Get(int id)
    {
      return "value";
    }

    #endregion
  }
}