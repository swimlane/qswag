#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using QSwagGenerator.Errors;
using QSwagWebApi.Controllers;
using QSwagWebApi.Models;
using Xunit;

#endregion

namespace QSwagTest
{
    public class WebApiTest
    {
        private SwaggerController Controller => new SwaggerController(_optionsWrapper);
        private readonly string _xmlDocPath = Path.GetFullPath("QSwagWebApi.xml");
        private readonly OptionsWrapper<Licenses> _optionsWrapper;

        public WebApiTest()
        {
            _optionsWrapper = new OptionsWrapper<Licenses>(new Licenses {Newtonsoft = GetLicense()});
        }

        private string GetLicense()
        {
            var license = string.Empty;
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
            var expected = File.ReadAllText(Path.Combine("Include", "MixNMatch.json"));
            AssertEqualIgnoreWhitespace(expected, result);
        }

        [Fact]
        public void CheckComplexType()
        {
            var result = Controller.GetSwagger("ComplexType", _xmlDocPath);
            var expected = File.ReadAllText(Path.Combine("Include", "ComplexType.json"));
            AssertEqualIgnoreWhitespace(expected, result);
        }

        [Fact]
        public void CheckDynamicType()
        {
            var result = Controller.GetSwagger("Dynamic", _xmlDocPath);
            var expected = File.ReadAllText(Path.Combine("Include", "Dynamic.json"));
            AssertEqualIgnoreWhitespace(expected, result);
        }

        [Fact]
        public void CheckNullablePath()
        {
            var result = Controller.GetSwagger("NullablePath", _xmlDocPath);
            var expected = File.ReadAllText(Path.Combine("Include", "NullablePath.json"));
            AssertEqualIgnoreWhitespace(expected, result);
        }

        [Fact]
        public void CheckSharedRoute()
        {
            var result = Controller.GetMultiTypeSwagger(new List<string> {"SplitOneController", "SplitTwoController"},
                _xmlDocPath);
            var expected = File.ReadAllText(Path.Combine("Include", "SharedRoute.json"));
            AssertEqualIgnoreWhitespace(expected, result);
        }

        [Fact]
        public void CheckSharedRoute_Validate()
        {
            var exception = Assert.Throws<ValidationException>(() =>
                Controller.GetMultiTypeSwagger(new List<string> {"SplitOneController", "SplitThreeController"},
                    _xmlDocPath));
            AssertEqualIgnoreWhitespace("Duplicate method name.", exception.Message);
        }
        
        [Fact]
        public void CheckComplexListType()
        {
            var result = Controller.GetSwagger("ComplexListType", _xmlDocPath);
            var expected = File.ReadAllText(Path.Combine("Include", "ComplexListType.json"));
            AssertEqualIgnoreWhitespace(expected, result);
        }

        #endregion

        private void AssertEqualIgnoreWhitespace(string expected, string actual)
        {
            var expectedStripped = StripWhitespace(expected);
            var actualStripped = StripWhitespace(actual);
            Assert.Equal(expectedStripped, actualStripped);
        }

        private string StripWhitespace(string toBeStripped)
        {
            return Regex.Replace(toBeStripped, @"\s+", string.Empty);
        }
    }
}
