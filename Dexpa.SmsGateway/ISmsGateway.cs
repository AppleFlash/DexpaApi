namespace Dexpa.SmsGateway
{
    public interface ISmsGateway
    {
        void SendMessage(string phone, string message);
    }
}