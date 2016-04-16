using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Repositories;
using Dexpa.Core.Utils;

namespace Dexpa.Core.Services
{
    public class TariffsService : ITariffsService
    {
        private readonly ITariffRepository mTariffRepository;

        private readonly IRegionRepository mRegionRepository;

        public TariffsService(ITariffRepository tariffRepository, IRegionRepository regionRepository)
        {
            mTariffRepository = tariffRepository;
            mRegionRepository = regionRepository;
        }

        public Tariff GetTariff(long tarifId)
        {
            return mTariffRepository.Single(t => t.Id == tarifId);
        }

        public Tariff AddTarif(Tariff tariff)
        {
            if (!mTariffRepository.Any(c => c.Name == tariff.Name || c.Abbreviation == tariff.Abbreviation))
            {
                mTariffRepository.Add(tariff);
                //UpdateTariffRelations(tariff);
                mTariffRepository.Commit();
                return tariff;
            }
            else
            {
                return null;
            }
        }

        public void DeleteTarif(long tarifId)
        {
            var tarif = mTariffRepository.Single(t => t.Id == tarifId);
            if (tarif != null)
            {
                mTariffRepository.Delete(tarif);
                mTariffRepository.Commit();
            }
        }

        public IList<Tariff> GetTariffs()
        {
            return mTariffRepository.List();
        }

        public IList<LightTariff> GetLightTariff()
        {
            var tariffs = mTariffRepository.List();

            List<LightTariff> report = new List<LightTariff>();

            foreach (var tariff in tariffs)
            {
                report.Add(new LightTariff()
                {
                    Id = tariff.Id,
                    Name = tariff.Name,
                    Abbreviation = tariff.Abbreviation,
                    Days = tariff.Days,
                    MinimumCost = tariff.MinimumCost,
                    TimeFrom = tariff.TimeFrom,
                    TimeTo = tariff.TimeTo,
                    YandexId = tariff.YandexId
                });
            }

            return report;
        }

        public Tariff UpdateTarif(Tariff tariff)
        {
            mTariffRepository.Update(tariff);
            //UpdateTariffRelations(tariff);
            mTariffRepository.Commit();
            return tariff;
        }

        private void UpdateTariffRelations(Tariff tariff)
        {
            if (tariff.RegionsCosts != null)
            {
                foreach (var region in tariff.RegionsCosts)
                {
                    Region regionTo = mRegionRepository.Single(r => r.Id == region.RegionToId);
                    Region regionFrom = mRegionRepository.Single(r => r.Id == region.RegionFromId);
                    region.RegionTo = regionTo;
                    region.RegionFrom = regionFrom;
                }
            }
        }

        public Tariff GetActiveTariff(string yandexTariffId, DateTime departureTime)
        {
            var localTime = TimeConverter.UtcToLocal(departureTime);
            var tariffs = mTariffRepository.List(t => t.YandexId == yandexTariffId);
            var time = localTime.TimeOfDay;

            foreach (var tariff in tariffs)
            {
                if (!Utils.Utils.IsSameDay(tariff.Days, localTime.DayOfWeek))
                {
                    continue;
                }
                var timeFrom = TimeConverter.UtcToLocal(tariff.TimeFrom);
                var timeTo = TimeConverter.UtcToLocal(tariff.TimeTo);
                if (timeFrom > timeTo)
                {
                    if (time >= timeFrom || time <= timeTo)
                    {
                        return tariff;
                    }
                }
                else
                {
                    if (time >= timeFrom && time <= timeTo)
                    {
                        return tariff;
                    }
                }
            }

            return null;
        }

        public void Dispose()
        {
            mRegionRepository.Dispose();
            mTariffRepository.Dispose();
        }
    }
}
