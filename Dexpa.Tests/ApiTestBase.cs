using System;
using System.IO;
using System.Net;
using System.Text;

namespace Dexpa.Tests
{
    public class ApiTestBase
    {
        private string mApiServer = "http://localhost:41898/api/";

        protected string ExecuteRequest(string endpoint, Method method, long id, string content)
        {
            var url = mApiServer + endpoint;
            if (id > 0)
            {
                url += "/" + id;
            }
            var request = HttpWebRequest.Create(url);
            request.Method = method.ToString().ToUpper();
            request.ContentType = "application/json";
            if (content != null)
            {
                var bytes = Encoding.UTF8.GetBytes(content);
                request.ContentLength = bytes.Length;
                request.GetRequestStream().Write(bytes, 0, bytes.Length);
            }
            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var responseContent = reader.ReadToEnd();
                Console.Write(responseContent);
                return responseContent;
            }
        }

        protected string ExecuteRequest(string endpoint, Method method)
        {
            return ExecuteRequest(endpoint, method, -1, null);
        }

        protected string ExecuteRequest(string endpoint, Method method, long id)
        {
            return ExecuteRequest(endpoint, method, id, null);
        }

        protected string ExecuteRequest(string endpoint, Method method, string content)
        {
            return ExecuteRequest(endpoint, method, -1, content);
        }
    }

    public enum Method
    {
        Get,
        Post,
        Put,
        Delete
    }
}
