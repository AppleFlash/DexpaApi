namespace Dexpa.DTO
{
    public class DispatcherReportDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string OrdersType { get; set; }

        public int OrdersCount { get; set; }

        public string ClearanceTime { get; set; }

        public int Percent { get; set; }

        public int AnsweredCalls { get; set; }

        public int UnAnsweredCalls { get; set; }

        public int DoneCalls { get; set; }

        public string Inaction { get; set; }

        public int ClickCount { get; set; }

        public int SendMessages { get; set; }

        public int ReadMessages { get; set; }
    }
}