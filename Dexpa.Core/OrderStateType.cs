namespace Dexpa.Core
{
    public enum OrderStateType
    {
        Created = 0,
        /// <summary>
        /// Assigned to driver
        /// </summary>
        Assigned = 1,
        /// <summary>
        /// Driving to customer
        /// </summary>
        Driving = 2,
        /// <summary>
        /// Waiting customer
        /// </summary>
        Waiting = 3,
        /// <summary>
        /// Transporting customer
        /// </summary>
        Transporting = 4,
        /// <summary>
        /// Successfully completed
        /// </summary>
        Completed = 5,
        /// <summary>
        /// Rejected by driver, dispatcher
        /// </summary>
        Rejected = 6,
        /// <summary>
        /// Failed by driver
        /// </summary>
        Failed = 7,
        /// <summary>
        /// Cancelled by customer or yandex
        /// </summary>
        Canceled = 8,
        /// <summary>
        /// Driver accepted order
        /// </summary>
        Accepted = 9,
        /// <summary>
        /// Order provider (yandex) approved order assignment
        /// </summary>
        Approved = 10,
        /// <summary>
        /// Order provider (yandex) disapproved order assignment
        /// </summary>
        Disapproved = 11
    }
}
