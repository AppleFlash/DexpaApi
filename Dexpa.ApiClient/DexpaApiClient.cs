using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Dexpa.DTO;
using Dexpa.DTO.Events;
using Newtonsoft.Json;
using NLog;

namespace Dexpa.ApiClient
{
    public class DexpaApiClient
    {
        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();

        private string mServerUrl;

        private string mLogin;

        private string mPassword;

        private Token mToken;

        public DexpaApiClient(string serverUrl, string login, string password)
        {
            mServerUrl = serverUrl;
            mLogin = login;
            mPassword = password;
        }

        public DexpaApiClient(ApiCredentials credentials)
        {
            mServerUrl = credentials.ApiUrl;
            mLogin = credentials.Login;
            mPassword = credentials.Password;
        }

        public List<EventOrderStateChangedDTO> GetOrderEvents(DateTime timestamp)
        {
            var stringDate = DateTimeToString(timestamp);
            var parameter = "fromTimestamp=" + stringDate;
            var result = ExecuteRequest(ApiEndpoint.Events, Method.Get, parameter);
            return JsonConvert.DeserializeObject<List<EventOrderStateChangedDTO>>(result);
        }

        public List<DriverDTO> GetDrivers()
        {
            var result = ExecuteRequest(ApiEndpoint.Drivers, Method.Get);
            return JsonConvert.DeserializeObject<List<DriverDTO>>(result);
        }

        private string DateTimeToString(DateTime date)
        {
            return string.Format("{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}", date);
        }

        private string ExecuteRequest(ApiEndpoint endpoint, Method method, string parameters = null, string data = null)
        {
            try
            {
                var url = mServerUrl + endpoint;
                if (!string.IsNullOrEmpty(parameters))
                {
                    url += "/?" + parameters;
                }

                var request = HttpWebRequest.Create(url);
                request.Method = method.ToString().ToUpper();
                request.ContentType = "application/json";
                if (data != null)
                {
                    var bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;
                    request.GetRequestStream().Write(bytes, 0, bytes.Length);
                }
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseContent = reader.ReadToEnd();
                    return responseContent;
                }
            }
            catch (Exception exception)
            {
                mLogger.Error(exception);
                throw;
            }
        }


    }
}
