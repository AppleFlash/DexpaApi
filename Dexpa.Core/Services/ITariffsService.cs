using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;

namespace Dexpa.Core.Services
{
    public interface ITariffsService : IDisposable
    {
        Tariff GetTariff(long tarifId);

        Tariff GetActiveTariff(string yandexTariffId, DateTime departureTime);

        Tariff AddTarif(Tariff tariff);

        void DeleteTarif(long tarifId);

        IList<Tariff> GetTariffs();

        IList<LightTariff> GetLightTariff();
        Tariff UpdateTarif(Tariff tariff);
    }
}
