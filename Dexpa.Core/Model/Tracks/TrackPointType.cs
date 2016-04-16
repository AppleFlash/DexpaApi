using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Tracks
{
    public enum TrackPointType
    {
        Idle = 0,
        Driving = 1,
        NewOrder = 2,
        DrivingToClient = 3,
        WaitingClient = 4,
        TransportingClient = 5,
        OrderCompleted = 6,
        OrderCancelled = 7,
        OrderFailed = 8
    }
}
