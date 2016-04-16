using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IDriverWorkConditionsService : IDisposable
    {
        IList<DriverWorkConditions> GetWorkConditions();

        DriverWorkConditions AddWorkConditions(DriverWorkConditions conditions);

        DriverWorkConditions UpdateWorkConditions(DriverWorkConditions conditions);

        void DeleteWorkConditions(int conditions);

        DriverWorkConditions GetWorkConditions(long id);
    }
}