using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class GlobalSettingsService : IGlobalSettingsService
    {
        private IGlobalSettingsRepository mRepository;

        public DateTime mLastRentTime;

        public GlobalSettingsService(IGlobalSettingsRepository repository)
        {
            mRepository = repository;
        }

        public GlobalSettings GetSettings()
        {
            var settings = mRepository.List().FirstOrDefault();
            if (settings == null)
            {
                settings = new GlobalSettings();
                mRepository.Add(settings);
                mRepository.Commit();
            }

            return settings;
        }

        public void SaveSettings(GlobalSettings globalSettings)
        {
            mRepository.Update(globalSettings);
            mRepository.Commit();
        }

        public void Dispose()
        {
            mRepository.Dispose();
        }
    }
}
