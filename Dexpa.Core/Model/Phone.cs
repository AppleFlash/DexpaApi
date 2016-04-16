using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    [ComplexType]
    public class Phone
    {
        public long Number { get; set; }

        public int? Additional { get; set; }

        public override string ToString()
        {
            if (Additional.HasValue)
            {
                return string.Format("+7{0},{1}", Number, Additional);
            }
            return string.Format("+7{0}", Number);
        }
    }
}
