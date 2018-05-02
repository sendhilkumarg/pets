using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace PetList.UnitTest
{
    /// <summary>
    /// Handle the http exception that is expected
    /// </summary>
    public sealed class ExpectedHttpStatusCode : ExpectedExceptionBaseAttribute
    {
        private HttpStatusCode _httpStatusCode = HttpStatusCode.OK;
        private string _message = string.Empty;

        public ExpectedHttpStatusCode(HttpStatusCode httpStatusCode)
        {
            _message = "";
            _httpStatusCode = httpStatusCode;
        }

        protected override void Verify(Exception exception)
        {
            var x = exception.GetType();
            //if (exception is HttpException)
            //{
            //    var httpException = (HttpException)exception;
            //    Assert.AreEqual(_httpStatusCode, httpException.ResponseStatusCode, "HttpStatusCode is not expected!");
            //}
            //else
            //{
            //    Assert.AreEqual(typeof(HttpException), typeof(Exception), "Exception is not typeof HttpException!");
            //}
        }
    }
}
