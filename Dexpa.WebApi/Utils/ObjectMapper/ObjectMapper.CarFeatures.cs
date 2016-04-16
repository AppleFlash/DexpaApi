using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreatMapCarFeatures()
        {
            Mapper.CreateMap<CarFeatures, CarFeaturesDTO>()
                .ConvertUsing(CarFeaturesToDTO);

            Mapper.CreateMap<CarFeaturesDTO, CarFeatures>()
                .ConvertUsing(DTOToCarFeatures);
        }

        private CarFeatures DTOToCarFeatures(CarFeaturesDTO carFeaturesDTO)
        {
            var features = CarFeatures.None;
            if (carFeaturesDTO == null)
            {
                return features;
            }

            if (carFeaturesDTO.Bussiness)
            {
                features = features | CarFeatures.Bussiness;
            }
            if (carFeaturesDTO.Comfort)
            {
                features = features | CarFeatures.Comfort;
            }
            if (carFeaturesDTO.Economy)
            {
                features = features | CarFeatures.Economy;
            }
            if (carFeaturesDTO.Minivan)
            {
                features = features | CarFeatures.Minivan;
            }
            if (carFeaturesDTO.Smoke)
            {
                features = features | CarFeatures.Smoke;
            }
            if (carFeaturesDTO.StationWagon)
            {
                features = features | CarFeatures.StationWagon;
            }
            if (carFeaturesDTO.Wifi)
            {
                features = features | CarFeatures.Wifi;
            }
            if (carFeaturesDTO.WithAnimals)
            {
                features = features | CarFeatures.WithAnimals;
            }
            if (carFeaturesDTO.Conditioner)
            {
                features = features | CarFeatures.Conditioner;
            }
            if (carFeaturesDTO.Coupon)
            {
                features = features | CarFeatures.Coupon;
            }
            if (carFeaturesDTO.Receipt)
            {
                features = features | CarFeatures.Receipt;
            }

            return features;
        }

        private CarFeaturesDTO CarFeaturesToDTO(CarFeatures carFeatures)
        {
            return new CarFeaturesDTO
            {
                Bussiness = carFeatures.HasFlag(CarFeatures.Bussiness),
                Comfort = carFeatures.HasFlag(CarFeatures.Comfort),
                Economy = carFeatures.HasFlag(CarFeatures.Economy),
                Minivan = carFeatures.HasFlag(CarFeatures.Minivan),
                Smoke = carFeatures.HasFlag(CarFeatures.Smoke),
                StationWagon = carFeatures.HasFlag(CarFeatures.StationWagon),
                Wifi = carFeatures.HasFlag(CarFeatures.Wifi),
                WithAnimals = carFeatures.HasFlag(CarFeatures.WithAnimals),
                Conditioner = carFeatures.HasFlag(CarFeatures.Conditioner),
                Coupon = carFeatures.HasFlag(CarFeatures.Coupon),
                Receipt = carFeatures.HasFlag(CarFeatures.Receipt)
            };
        }
    }
}