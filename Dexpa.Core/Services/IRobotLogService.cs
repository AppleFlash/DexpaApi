using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IRobotLogService : IDisposable
    {
        RobotLog AddLog(RobotLog robotLog);
        IList<RobotLog> AddLogs(IList<RobotLog> robotLogs);
        IList<RobotLog> GetLogs(long orderId);
    }
}