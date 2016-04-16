using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class RobotLogsController : ApiControllerBase
    {
        private IRobotLogService mRobotLogService;

        public RobotLogsController(IRobotLogService service)
        {
            mRobotLogService = service;
        }

        public IList<RobotLogDTO> GetLogs(long orderId)
        {
            var logs = mRobotLogService.GetLogs(orderId);
            var logsDto = ObjectMapper.Instance.Map<IList<RobotLog>, List<RobotLogDTO>>(logs);
            return logsDto;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mRobotLogService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}