using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    [ComplexType]
    public class OrderOptions
    {
        public CarFeatures CarFeatures { get; set; }

        public ChildrenSeat ChildrenSeat { get; set; }
    }
}
