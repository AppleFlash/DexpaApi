using YAXLib;

namespace Yandex.Taxi.Model.Rates
{
    public class Payment
    {
        [YAXValueForClass]
        public decimal Price { get; set; }

        [YAXAttributeForClass]
        public PaymentType Type { get; set; }
    }
}