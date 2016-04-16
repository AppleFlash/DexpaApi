using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    [ComplexType]
    public class CarPermission
    {
        public string Number { get; set; }

        public string Series { get; set; }

        public string Number2 { get; set; }
    }
}