#region Using

using System.IO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using QSwagWebApi.Controllers;
using SwaggerSchema;
using Xunit;

#endregion

namespace QSwagTest
{
    public class WebApiTest
    {
        private readonly SwaggerController _controller;

        public WebApiTest()
        {
            _controller = new SwaggerController();
            //var controllerContextMock = new Mock<ControllerContext>();
            //controllerContextMock.Setup(ht => ht.HttpContext.Request.Host).Returns(null);
            //controllerContextMock.Setup(ht => ht.HttpContext.Request.Scheme).Returns("http");
            //_controller.ControllerContext = controllerContextMock.Object;
        }

        #region Access: Public

        [Fact]
        public void CheckMixNMatch()
        {
            var result = _controller.GetSwagger("MixNMatch");
            var expected = File.ReadAllText("Include\\MixNMatch.json");
            Assert.Equal(expected, result);
        }

        #endregion
    }
}