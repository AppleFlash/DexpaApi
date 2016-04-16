using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using Dexpa.WebApi.Models;

namespace Dexpa.WebApi.Utils
{
    public class DriverResult : IHttpActionResult
    {
        private Error mError;

        private HttpStatusCode mStatusCode;

        private HttpRequestMessage mRequest;

        public DriverResult(HttpStatusCode status, HttpRequestMessage request, Error error = null)
        {
            mStatusCode = status;
            mRequest = request;
            mError = error;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                RequestMessage = mRequest,
                StatusCode = mStatusCode
            };
            if (mError != null)
            {
                var json = Json.Encode(mError);
                response.Content = new StringContent(mError.LocalizedMessage);
            }
            return Task.FromResult(response);
        }
    }
}