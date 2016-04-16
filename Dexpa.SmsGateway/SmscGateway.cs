namespace Dexpa.SmsGateway
{
    internal class SmscGateway : ISmsGateway
    {
        private const char PHONES_SEPARATOR = ',';

        private SMSC mSmsService;

        public SmscGateway(string sSmsServiceLogin, string sSmsServicePassword)
        {
            mSmsService = new SMSC(sSmsServiceLogin, sSmsServicePassword);
        }

        public void SendMessage(string phone, string message)
        {
            mSmsService.send_sms(phone, message);
        }
    }
}
