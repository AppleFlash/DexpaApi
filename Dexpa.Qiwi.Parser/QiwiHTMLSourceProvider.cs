using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Qiwi.Parser
{
    public class QiwiHTMLSourceProvider : IHTMLSourceProvider
    {
        private CookieCollection mSessionCookies;

        public bool Login(string sLogin, string sPassword, out string sMessage)
        {
            string sLoginToken = GetLoginToken(sLogin, sPassword);
            bool bOk = Login(sLogin, sPassword, sLoginToken, out sMessage);
            if (bOk)
            {
                SwitchToEnglishLanguage();
            }
            return bOk;

        }

        private void SwitchToEnglishLanguage()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://visa.qiwi.com/system/empty.action");
            request.Accept = "text/html, */*; q=0.01";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Method = "POST";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ServicePoint.Expect100Continue = false;
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(mSessionCookies);

            using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write("lang=en");
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                string sResponse = sr.ReadToEnd();
            }
        }

        private bool Login(string sLogin, string sPassword, string sLoginToken, out string sMessage)
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://visa.qiwi.com/auth/login.action");
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Method = "POST";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ServicePoint.Expect100Continue = false;
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(mSessionCookies);

            using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write("login={0}&password={1}&loginToken={2}", WebUtility.UrlEncode(sLogin), WebUtility.UrlEncode(sPassword), WebUtility.UrlEncode(sLoginToken));
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            mSessionCookies = response.Cookies;

            string sResponse;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                sResponse = sr.ReadToEnd();
                dynamic jsonResponse = JObject.Parse(sResponse);
                sMessage = jsonResponse.message;
                return string.Compare((string)jsonResponse.code._name, "NORMAL", true) == 0;
            }
        }

        private string GetLoginToken(string sLogin, string sPassword)
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://visa.qiwi.com/auth/login.action");
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Method = "POST";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.ServicePoint.Expect100Continue = false;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.CookieContainer = new CookieContainer();

            string sBody = string.Format("login={0}&password={1}", WebUtility.UrlEncode(sLogin), WebUtility.UrlEncode(sPassword));

            using (StreamWriter sw = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
            {
                sw.Write(sBody);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            mSessionCookies = response.Cookies;

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                string sResponse = sr.ReadToEnd();
                dynamic jsonResponse = JObject.Parse(sResponse);
                return jsonResponse.data.token;
            }
        }

        public string GetTransactions(DateTime dtFrom, DateTime dtTo, string sType = null, string sStatus = null)
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://visa.qiwi.com/user/report/list.action");
            request.Accept = "text/html, */*; q=0.01";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Method = "POST";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ServicePoint.Expect100Continue = false;
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(mSessionCookies);

            string sRequestBody = ConstructRequestBody(dtFrom, dtTo, sType, sStatus);

            using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(sRequestBody);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        private static string ConstructRequestBody(DateTime dtFrom, DateTime dtTo, string sType = null, string sStatus = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("daterange=true&start={0}&finish={1}", dtFrom, dtTo);

            if (sType != null || sStatus != null)
            {
                sb.Append("&settings=true&");

                if (sType != null && sStatus != null)
                {
                    sb.AppendFormat("conditions.directions={0}&conditions.status={1}", sType, sStatus);
                }
                else if (sType != null)
                {
                    sb.AppendFormat("conditions.directions={0}", sType);
                }
                else
                {
                    sb.AppendFormat("conditions.status={0}", sStatus);
                }
            }

            return sb.ToString();
        }
    }
}
