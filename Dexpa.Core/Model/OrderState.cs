using System;

namespace Dexpa.Core.Model
{
    public class OrderState
    {
        public DateTime CreatedTime { get; set; }

        public DateTime AssignedTime { get; set; }

        public DateTime DrivingTime { get; set; }

        public DateTime WaitingTime { get; set; }

    }
}
