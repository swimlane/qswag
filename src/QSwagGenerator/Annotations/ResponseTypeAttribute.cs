using System;

namespace QSwagGenerator.Annotations
{
     [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ResponseTypeAttribute : Attribute
    {
        public ResponseTypeAttribute(Type responseType)
        {
            HttpStatusCode = "200";
            ResponseType = responseType;
        }
        public ResponseTypeAttribute(string httpStatusCode, Type responseType)
        {
            HttpStatusCode = httpStatusCode;
            ResponseType = responseType;
        }
        public string HttpStatusCode { get; set; }
       public Type ResponseType { get; set; }
    }
}
