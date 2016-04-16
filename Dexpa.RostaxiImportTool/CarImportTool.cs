using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using Dexpa.Core.Model;
using Dexpa.Infrastructure.Repositories;

namespace Dexpa.RostaxiImportTool
{
    class CarImportTool : ImportToolBase
    {
        public CarImportTool(string filename, DbContext context)
            : base(filename, context)
        {
        }

        protected override void ImportData()
        {
            var carRepository = new CarRepository(mDbContext);
            //Позывной	Марка	Модель	Категория	К	Год	Цвет	Гос. номер	Статус	Пробег	ТО	ОСАГО	КАСКО	Лицензия	Создан	Детское кресло	Дополнительные опции

            using (var streamRead = new StreamReader(mFileName))
            {
                var header = streamRead.ReadLine();
                while (!streamRead.EndOfStream)
                {
                    var line = streamRead.ReadLine();
                    var values = line.Split('\t');

                    var callsign = values[0].Trim();
                    var brand = values[1].Trim();
                    var model = values[2].Trim();
                    var featureList = ParseFeatures(values[3], values[16]);
                    var carClass = (CarClass)Enum.Parse(typeof(CarClass), values[4], ignoreCase: true);
                    var year = int.Parse(values[5]);
                    var color = values[6].Trim();
                    var regNumber = values[7].Trim();
                    var status = ParseStatus(values[8].Trim());
                    var created = DateTime.Parse(values[14], CultureInfo.GetCultureInfo("ru"));
                    var childSeat = (ChildrenSeat)int.Parse(values[15]);

                    Car car = new Car(created)
                    {
                        CarClass = carClass,
                        ChildrenSeat = childSeat,
                        Callsign = callsign,
                        Features = featureList,
                        ProductionYear = year,
                        Model = model,
                        Brand = brand,
                        RegNumber = regNumber,
                        Color = color,
                        Status = status,
                    };
                    carRepository.Add(car);

                    RecordsImported++;
                }
            }
        }

        private CarStatus ParseStatus(string status)
        {
            if (status == "Работает")
            {
                return CarStatus.Works;
            }
            return CarStatus.None;
        }

        private CarFeatures ParseFeatures(string categories, string options)
        {
            var result = CarFeatures.None;
            if (categories.Contains("Комфорт"))
            {
                result |= CarFeatures.Comfort;
            }
            if (categories.Contains("Эконом"))
            {
                result |= CarFeatures.Economy;
            }
            if (categories.Contains("Универсал"))
            {
                result |= CarFeatures.StationWagon;
            }
            if (categories.Contains("Бизнес"))
            {
                result |= CarFeatures.Bussiness;
            }
            if (categories.Contains("Минивен"))
            {
                result |= CarFeatures.Minivan;
            }

            if (options.Contains("WiFi"))
            {
                result |= CarFeatures.Wifi;
            }
            if (options.Contains("Кондиционер"))
            {
                result |= CarFeatures.Conditioner;
            }
            if (options.Contains("Перевозка животных"))
            {
                result |= CarFeatures.WithAnimals;
            }
            if (options.Contains("Курить"))
            {
                result |= CarFeatures.Smoke;
            }

            return result;
        }
    }
}
