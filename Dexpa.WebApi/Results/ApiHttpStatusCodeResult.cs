using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Dexpa.WebApi.Results
{
    /// <summary>
    /// This class is a replacement for HttpStatusCodeResult in Trakopolis API server. 
    /// It should be used when API server returns an error to the API client.
    /// Difference with HttpStatusCodeResult is that this class doesn't return any HTML, just status code and status description.
    /// </summary>
    public class ApiHttpStatusCodeResult : ActionResult
    {
        private readonly HttpStatusCode mStatusCode;
        private readonly string mStatusDescription;

        public ApiHttpStatusCodeResult(HttpStatusCode statusCode, String statusDescription = null)
        {
            mStatusCode = statusCode;
            mStatusDescription = statusDescription;
        }

        public ApiHttpStatusCodeResult(HttpStatusCode statusCode, String statusDescriptionFormat, params object[] args)
            : this(statusCode, String.Format(statusDescriptionFormat, args))
        { }

        public Int32 StatusCode
        {
            get { return (Int32)mStatusCode; }
        }

        public string StatusDescription
        {
            get { return mStatusDescription; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.HttpContext != null && context.HttpContext.Response != null)
            {
                context.HttpContext.Response.StatusCode = StatusCode;
                context.HttpContext.Response.StatusDescription = StatusDescription;
                if (HttpContext.Current != null) // Do not show custom IIS errors.
                    HttpContext.Current.Response.TrySkipIisCustomErrors = true;

                new EmptyResult().ExecuteResult(context);
            }
            else // Fallback to HttpStatusCode result in weird situation.
                (new HttpStatusCodeResult(StatusCode, StatusDescription)).ExecuteResult(context);
        }
    }
}