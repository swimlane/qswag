#region Using

using System;
using System.Collections.Generic;
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
            var license = "3236-jzVn+ETyq5H+aLEfScZNsZvmQiMbQDRc6SDkv9ToEWdfUBKOvDEE0oXbMm34Othi/i8So/18DvygQioa0m+84kIHaB2bqgHyLAjRsXs09cK24C+0NySgz4VB6n3DMi0124alnlTnfkmp/sM08bzJjEuf6mw1EOfMg1GlRE2p21B7IklkIjozMjM2LCJFeHBpcnlEYXRlIjoiMjAxNy0wNi0wOFQxOTo1MDoyOS4xNDUyNzgxWiIsIlR5cGUiOiJKc29uU2NoZW1hQnVzaW5lc3MifQ=="; //Your newtonsoft schema key
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

      [Fact]
        public void CheckSharedRoute()
        {
            var result = Controller.GetMultiTypeSwagger(new List<string>{"SplitOneController", "SplitTwoController"}, _xmlDocPath);
            var expected = File.ReadAllText("Include\\SharedRoute.json");
            Assert.Equal(expected, result);
        }
        #endregion
    }
}