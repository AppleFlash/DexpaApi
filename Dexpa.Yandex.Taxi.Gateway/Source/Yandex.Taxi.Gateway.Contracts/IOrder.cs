using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Gateway.Contracts
{
    public interface IOrder
    {
        string Id { get; }

        string SourceAddress { get; }

        string DestinationAddress { get; }

        DateTime BookingTime { get; }
    }
}
