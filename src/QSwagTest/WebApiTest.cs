#region Using

using System;
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
        private  SwaggerController Controller => new SwaggerController(_optionsWrapper);
        private readonly string _xmlDocPath = Path.GetFullPath("QSwagWebApi.xml");
        private readonly OptionsWrapper<Licenses> _optionsWrapper;

        public WebApiTest()
        {
            _optionsWrapper = new OptionsWrapper<Licenses>(new Licenses {Newtonsoft = GetLicense()});
            //var controllerContextMock = new Mock<ControllerContext>();
            //controllerContextMock.Setup(ht => ht.HttpContext.Request.Host).Returns(null);
            //controllerContextMock.Setup(ht => ht.HttpContext.Request.Scheme).Returns("http");
            //_controller.ControllerContext = controllerContextMock.Object;
        }

        private string GetLicense()
        {
            var license = string.Empty; //Your newtonsoft schema key
            try
            {
                 license = Environment.GetEnvironmentVariable("Newtonsoft");
            }
            catch(ArgumentNullException)
            {
                
            }
            return license;
        }
        #region Access: Public

        [Fact]
        public void CheckMixNMatch()
        {
            var result = Controller.GetSwagger("MixNMatch", _xmlDocPath);
            var expected = File.ReadAllText("Include\\MixNMatch.json");
            Assert.Equal(expected, result);
        }
        [Fact]
        public void CheckComplexType()
        {
            var result = Controller.GetSwagger("ComplexType", _xmlDocPath);
            var expected = File.ReadAllText("Include\\ComplexType.json");
            Assert.Equal(expected, result);
        }
        [Fact]
        public void CheckDynamicType()
        {
            var result = Controller.GetSwagger("Dynamic", _xmlDocPath);
            var expected = File.ReadAllText("Include\\Dynamic.json");
            Assert.Equal(expected, result);
        }
        [Fact]
        public void CheckNullablePath()
        {
            var result = Controller.GetSwagger("NullablePath", _xmlDocPath);
            var expected = File.ReadAllText("Include\\NullablePath.json");
            Assert.Equal(expected, result);
        }
        #endregion
    }
}