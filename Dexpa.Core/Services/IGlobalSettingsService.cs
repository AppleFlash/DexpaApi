using System;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IGlobalSettingsService : IDisposable
    {
        GlobalSettings GetSettings();

        void SaveSettings(GlobalSettings globalSettings);
    }
}