using System;
using System.Collections.Generic;
using System.Web.Http;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Reports;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.DTO.HelpDictionaries;
using Dexpa.WebApi.Utils;
using Dexpa.Core.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class CarEventReportController : ApiControllerBase
    {
        private ICarEventReportService mCarEventReportService;

        public CarEventReportController(ICarEventReportService carEventReportService)
        {
            mCarEventReportService = carEventReportService;
        }

        [HttpGet]
        public IEnumerable<CarEventReportDTO> CarEventsReport(long carId)
        {
            return
                ObjectMapper.Instance.Map<IList<CarEventReport>, List<CarEventReportDTO>>(
                    mCarEventReportService.GetCarEventsReport(carId));
        }
    }
}
