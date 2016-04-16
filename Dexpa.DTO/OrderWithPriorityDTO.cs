using Dexpa.DTO;

namespace Dexpa.Core.Model
{
    public class OrderWithPriorityDTO
    {
        public OrderDTO Order { get; set; }
        public OrderPriority Priority { get; set; }
    }
}
