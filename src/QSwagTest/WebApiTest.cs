#region Using

using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QSwagWebApi.Controllers;
using QSwagWebApi.Models;
using Xunit;

#endregion

namespace QSwagTest
{
    public class WebApiTest
    {
        private readonly SwaggerController _controller;
        private readonly string xmlDocPath = Path.GetFullPath("QSwagWebApi.xml");

        public WebApiTest()
        {
            var key = "your newtonsoft key";
            var optionsWrapper = new OptionsWrapper<Licenses>(new Licenses {Newtonsoft = key});
            _controller = new SwaggerController(optionsWrapper);
            //var controllerContextMock = new Mock<ControllerContext>();
            //controllerContextMock.Setup(ht => ht.HttpContext.Request.Host).Returns(null);
            //controllerContextMock.Setup(ht => ht.HttpContext.Request.Scheme).Returns("http");
            //_controller.ControllerContext = controllerContextMock.Object;
        }

        #region Access: Public

        [Fact]
        public void CheckMixNMatch()
        {
            var result = _controller.GetSwagger("MixNMatch", xmlDocPath);
            var expected = File.ReadAllText("Include\\MixNMatch.json");
            Assert.Equal(expected, result);
        }
        [Fact]
        public void CheckComplexType()
        {
            var result = _controller.GetSwagger("ComplexType", xmlDocPath);
            var expected = File.ReadAllText("Include\\ComplexType.json");
            Assert.Equal(expected, result);
        }
        #endregion
    }
}