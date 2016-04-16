using System.Text;
using Dexpa.Core.Model;

namespace Dexpa.Core.Factories
{
    public class DriverFactory
    {
        public Driver Create(string firstName, string lastName, string middleName, string callsign, params long[] phones)
        {
            return new Driver()
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Phones = FormatPhohones(phones),
            };
        }

        private string FormatPhohones(long[] phones)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < phones.Length; i++)
            {
                builder.AppendFormat("{0}, ", phones[i]);
            }
            builder = builder.Remove(builder.Length - 2, 2);
            return builder.ToString();
        }
    }
}
