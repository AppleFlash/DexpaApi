using System;

namespace Dexpa.DTO
{
    public class DriversReportDTO
    {
        public long Id { get; set; }

        public string DriverWorkConditions { get; set; }

        public string DriverCallsign { get; set; }

        public string DriverName { get; set; }

        public double Amount { get; set; }

        public double Percent { get; set; }

        public double Rent { get; set; }

        public double Debt { get; set; }

        public double TechSupport { get; set; }

        public double TaxometrAmount { get; set; }

        public int AllOrders { get; set; }

        public int DoneOrders { get; set; }

        public int ClientCanceled { get; set; }

        public int DriverCanceled { get; set; }

        public int Rating { get; set; }

        public int AverageRating { get; set; }

        public int PartGoodTrack { get; set; }

        public DateTime Timestamp { get; set; }

    }
}