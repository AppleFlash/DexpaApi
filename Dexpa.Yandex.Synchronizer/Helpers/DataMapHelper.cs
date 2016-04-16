using Dexpa.Core.Model;
using System;
using System.Collections.Generic;
using Yandex.Taxi.Model.Drivers;
using Yandex.Taxi.Model.Orders;
using Car = Dexpa.Core.Model.Car;
using Order = Yandex.Taxi.Model.Orders.Order;
using Requirement = Yandex.Taxi.Model.Drivers.Requirement;
using RequirementType = Yandex.Taxi.Model.Drivers.RequirementType;

namespace Yandex.Synchronizer.Helpers
{
    public static class DataMapHelper
    {
        public static Cars MapDrivers(System.Collections.Generic.IList<Driver> drivers)
        {
            Cars cars = new Cars();
            foreach (Driver driver in drivers)
            {
                Car car = driver.Car;
                if (car != null)
                {
                    var yaCar = new Taxi.Model.Drivers.Car();
                    yaCar.Uuid = driver.Id.ToString();
                    if (car.Features.HasFlag(CarFeatures.Economy))
                    {
                        yaCar.Tariffs.Add("econom");
                    }
                    if (car.Features.HasFlag(CarFeatures.Comfort))
                    {
                        yaCar.Tariffs.Add("business");
                    }
                    if (car.Features.HasFlag(CarFeatures.Bussiness))
                    {
                        yaCar.Tariffs.Add("vip");
                    }
                    yaCar.DriverDetails.DisplayName = string.Format("{0} {1} {2}", driver.LastName, driver.FirstName, driver.MiddleName);
                    yaCar.DriverDetails.DriverLicense = driver.DriverLicense != null ? string.Format("{0}{1}", driver.DriverLicense.Series,
                        driver.DriverLicense.Number) : string.Empty;
                    yaCar.DriverDetails.Phone = GetPhone(driver.Phones);
                    yaCar.DriverDetails.Age = 1980;
                    CarPermission permission = car.Permission;
                    yaCar.DriverDetails.Permit = string.Format("{0} {1} {2}", permission.Number, permission.Series, permission.Number2);
                    yaCar.CarDetails.Model = string.Format("{0} {1}", car.Brand, car.Model);
                    yaCar.CarDetails.CarNumber = car.RegNumber;
                    yaCar.CarDetails.Color = car.Color;
                    yaCar.CarDetails.Age = car.ProductionYear;
                    var requirements = yaCar.CarDetails.Requirements;

                    var requirement = new Requirement();
                    requirement.Type = RequirementType.AnimalTransport;
                    requirement.Value = car.Features.HasFlag(CarFeatures.WithAnimals) ? "yes" : "no";
                    requirements.Add(requirement);

                    requirement = new Requirement();
                    requirement.Type = RequirementType.Check;
                    requirement.Value = "no";
                    requirements.Add(requirement);

                    requirement = new Requirement();
                    requirement.Type = RequirementType.ChildChair;
                    requirement.Value = GetChildChair(car.ChildrenSeat);
                    requirements.Add(requirement);

                    requirement = new Requirement();
                    requirement.Type = RequirementType.HasConditioner;
                    requirement.Value = car.Features.HasFlag(CarFeatures.Conditioner) ? "yes" : "no";
                    requirements.Add(requirement);

                    requirement = new Requirement();
                    requirement.Type = RequirementType.NoSmoking;
                    requirement.Value = !car.Features.HasFlag(CarFeatures.Smoke) ? "yes" : "no";
                    requirements.Add(requirement);

                    requirement = new Requirement();
                    requirement.Type = RequirementType.Universal;
                    requirement.Value = car.Features.HasFlag(CarFeatures.StationWagon) ? "yes" : "no";
                    requirements.Add(requirement);

                    cars.Add(yaCar);
                }
            }
            return cars;
        }

        private static string GetPhone(string phones)
        {
            if (!string.IsNullOrEmpty(phones))
            {
                var phonesList = phones.Split(',');
                if (phonesList.Length > 0)
                {
                    var phone = phonesList[0].Trim();
                    if (phone.Length > 0)
                    {
                        phone = phone[0] == '8' ?
                            "+7" + phone.Substring(1, phone.Length - 1) :
                            phone[0] == '+' ? phone :
                            "+" + phone;

                        return phone;
                    }
                }
            }
            return "";
        }

        private static string GetChildChair(ChildrenSeat childrenSeat)
        {
            switch (childrenSeat)
            {
                case ChildrenSeat.None:
                    return "no";
                case ChildrenSeat.Weight0_10:
                    return "0-1";
                case ChildrenSeat.Weight0_13:
                    return "0-2";
                case ChildrenSeat.Weight0_20:
                    return "0-5";
                case ChildrenSeat.Weight0_25:
                    return "0-7";
                case ChildrenSeat.Weight0_40:
                    return "0-12";
                case ChildrenSeat.Weight9_18:
                    return "1-4";
                case ChildrenSeat.Weight9_36:
                    return "1-10";
                case ChildrenSeat.Weight15_25:
                    return "3-7";
                case ChildrenSeat.Weight22_36:
                    return "6-10";
                default:
                    throw new ArgumentOutOfRangeException("childrenSeat");
            }
        }

        public static Dexpa.Core.Model.Order MapOrder(Order order)
        {
            Address toAddress = null;
            if (order.Destinations.Count > 0)
            {
                toAddress = GetToAddress(order.Destinations[order.Destinations.Count - 1]);
            }
            var dexpaOrder = new Dexpa.Core.Model.Order();
            dexpaOrder.DepartureDate = order.BookingTime.Time;
            dexpaOrder.ToAddress = toAddress;
            dexpaOrder.FromAddress = GetFromAddres(order.Source);
            dexpaOrder.Source = OrderSource.Yandex;
            dexpaOrder.SourceOrderId = order.Id;
            dexpaOrder.Comments = order.Comments;
            return dexpaOrder;
        }

        private static Address GetFromAddres(Source source)
        {
            var address = new Address();
            var locality = source.Country.Locality;
            if (locality != null)
            {
                address.City = locality.Name;
                var thoroughfare = locality.Thoroughfare;
                if (thoroughfare != null)
                {
                    address.Street = thoroughfare.Name;

                    if (thoroughfare.Premise != null)
                    {
                        address.House = thoroughfare.Premise.Number;
                    }
                }
            }
            return address;
        }


        private static Address GetToAddress(Destination destination)
        {
            var address = new Address();
            var locality = destination.Country.Locality;
            if (locality != null)
            {
                address.City = locality.Name;
                var thoroughfare = locality.Thoroughfare;
                if (thoroughfare != null)
                {
                    address.Street = thoroughfare.Name;

                    if (thoroughfare.Premise != null)
                    {
                        address.House = thoroughfare.Premise.Number;
                    }
                }
            }
            return address;
        }
    }
}
