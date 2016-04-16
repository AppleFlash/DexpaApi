using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface ICarEventReportService
    {
        IList<CarEventReport> GetCarEventsReport(long carId);
    }
}