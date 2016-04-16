using System.Security.Cryptography;
using System.Text;

namespace Dexpa.Infrastructure.Utils
{
    public class HexIdGenerator
    {
        private static RNGCryptoServiceProvider mRandom = new RNGCryptoServiceProvider();

        public string Generate()
        {
            byte[] id = new byte[16];
            mRandom.GetBytes(id);
            return ToHex(id);
        }

        private static string ToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bytes[i]);
            }
            return sb.ToString();
        }
    }
}