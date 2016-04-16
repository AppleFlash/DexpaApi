using System.Configuration;

namespace Dexpa.SmsGateway
{
    public class SmsGatewayFactory
    {
        public static ISmsGateway CreateSmsGateway(string login, string password)
        {
            return new SmscGateway(login, password);
        }
    }
}
