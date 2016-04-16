namespace Dexpa.Core.Model
{
    public enum OrderRequestState
    {
        /// <summary>
        /// Just created
        /// </summary>
        New,
        /// <summary>
        /// Rejected by driver
        /// </summary>
        Rejected,
        /// <summary>
        /// Cancelled by yandex
        /// </summary>
        Cancelled
    }
}