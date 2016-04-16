using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.ApiClient;

namespace Dexpa.ApiClientTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DexpaApiClient("http://testapi.dexpa.ru/api/", null, null);
            var events = client.GetOrderEvents(new DateTime());
        }
    }
}
