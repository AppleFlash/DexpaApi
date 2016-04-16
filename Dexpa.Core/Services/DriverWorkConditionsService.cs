using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class DriverWorkConditionsService : IDriverWorkConditionsService
    {
        private IDriverWorkConditionsRepository mConditionsRepository;

        private IDriverRepository mDriverRepository;

        public DriverWorkConditionsService(IDriverWorkConditionsRepository conditionsRepository, IDriverRepository driverRepository)
        {
            mConditionsRepository = conditionsRepository;
            mDriverRepository = driverRepository;
        }

        public IList<DriverWorkConditions> GetWorkConditions()
        {
            return mConditionsRepository.List();
        }

        public DriverWorkConditions GetWorkConditions(long id)
        {
            return mConditionsRepository.Single(c => c.Id == id);
        }

        public DriverWorkConditions AddWorkConditions(DriverWorkConditions conditions)
        {
            if (!mConditionsRepository.Any(c => c.Name == conditions.Name))
            {
                CheckOrdersFeeList(conditions.OrderFees);
                conditions = mConditionsRepository.Add(conditions);
                mConditionsRepository.Commit();
                return conditions;
            }
            else
            {
                return null;
            }

        }

        public DriverWorkConditions UpdateWorkConditions(DriverWorkConditions conditions)
        {
            CheckOrdersFeeList(conditions.OrderFees);
            conditions = mConditionsRepository.Update(conditions);
            mConditionsRepository.Commit();
            return conditions;
        }

        public void DeleteWorkConditions(int id)
        {
            var conditions = mConditionsRepository.Single(c => c.Id == id);
            if (conditions != null)
            {
                if (mDriverRepository.Any(d => d.WorkConditionsId != null && d.WorkConditionsId == conditions.Id))
                {
                    throw new CoreException("Can't delete driver work contions. It's related with a one or more drivers", ErrorCode.Custom);
                }
                //conditions.OrderFees.Clear();
                mConditionsRepository.Delete(conditions);
                mConditionsRepository.Commit();
            }
        }

        private void CheckOrdersFeeList(ICollection<OrderFee> orderFees)
        {
            foreach (var orderFee in orderFees)
            {
                if (orderFees.Count(f => f.OrderType == orderFee.OrderType) > 1)
                {
                    throw new CoreException("Driver work conditions can't contains more than one order fee for each order type", ErrorCode.Custom);
                }
            }
        }

        public void Dispose()
        {
            mDriverRepository.Dispose();
            mConditionsRepository.Dispose();
        }
    }
}
