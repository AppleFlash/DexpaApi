using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class RobotLogService : IRobotLogService
    {
        private IRobotLogRepository mRobotLogRepository;

        public RobotLogService(IRobotLogRepository robotLogRepository)
        {
            mRobotLogRepository = robotLogRepository;
        }

        public RobotLog AddLog(RobotLog robotLog)
        {
            mRobotLogRepository.Add(robotLog);
            mRobotLogRepository.Commit();
            return robotLog;
        }

        public IList<RobotLog> AddLogs(IList<RobotLog> robotLogs)
        {
            mRobotLogRepository.AddLogs(robotLogs);
            mRobotLogRepository.Commit();
            return robotLogs;
        }

        public IList<RobotLog> GetLogs(long orderId)
        {
            return mRobotLogRepository.List(l => l.OrderId == orderId);
        }

        public void Dispose()
        {
            mRobotLogRepository.Dispose();
        }
    }
}
