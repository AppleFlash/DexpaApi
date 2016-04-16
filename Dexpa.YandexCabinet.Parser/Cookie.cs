using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.YandexCabinet.Parser
{
    public class Cookie
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public static List<Cookie> GetCookies(string cookie)
        {
            List<Cookie> cookies = new List<Cookie>();
            var buffer = cookie.Split(';');
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = buffer[i].Trim();
                var item = buffer[i].Split('=');
                cookies.Add(new Cookie()
                {
                    Name = item[0],
                    Value = item[1]
                });
            }

            return cookies;
        }
    }
}
