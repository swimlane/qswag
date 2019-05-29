#region Using

using Microsoft.AspNetCore.Mvc;
using QSwagWebApi.Models;

#endregion

namespace QSwagWebApi.Controllers
{
    [Route("api/ComplexListType")]
    public class ComplexListTypeController : Controller
    {
        #region Access: Public

        [HttpGet("chartsort/{id}")]
        public ChartSort GetChartSort(int id)
        {
            return new ChartSort();
        }

        #endregion
    }
}
