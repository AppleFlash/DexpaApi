using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class CarEventReportService : ICarEventReportService
    {
        private readonly ICarRepository mCarRepository;
        private readonly ICarEventRepository mCarEventRepository;
        private readonly IRepairRepository mRepairRepository;
        private readonly IWayBillsRepository mWayBillsRepository;

        public CarEventReportService(ICarRepository carRepository, ICarEventRepository carEventRepository, IRepairRepository repairRepository, IWayBillsRepository wayBillsRepository)
        {
            mCarRepository = carRepository;
            mCarEventRepository = carEventRepository;
            mRepairRepository = repairRepository;
            mWayBillsRepository = wayBillsRepository;
        }

        public IList<CarEventReport> GetCarEventsReport(long carId)
        {
            var idCount = 0;

            var report = new List<CarEventReport>();

            var car = mCarRepository.Single(c => c.Id == carId);

            var carCreationEvent = new CarEventReport()
            {
                Id = idCount++,
                Timestamp = car.Timestamp,
                Comment = "ТС добавлено в базу",
                Name = "Создание"
            };

            var carEvents = mCarEventRepository.List(e => e.CarId == carId).Select(e=> new CarEventReport()
            {
                Id = idCount++,
                Timestamp = e.Timestamp,
                Name = e.Name,
                Comment = e.Comment,
                CarEventId = e.Id
            });
            var carRepairs = mRepairRepository.List(e => e.CarId == carId).Select(e=>new CarEventReport()
            {
                Id = idCount++,
                Timestamp = e.Timestamp,
                Name = "Ремонт",
                Comment = e.Comment,
                RepairId = e.Id
            });
            var wayBills = mWayBillsRepository.List(e => e.CarId == carId).Select(e=>GetWayBillEvent(e, idCount));

            report.Add(carCreationEvent);
            report.AddRange(carEvents);
            report.AddRange(carRepairs);
            report.AddRange(wayBills);

            return report.OrderBy(e=>e.Timestamp).ToList();
        }

        private CarEventReport GetWayBillEvent(WayBills wb, long counter)
        {
            var report = new CarEventReport();
            var comment = "Выдан путевой лист [" + wb.Car.Callsign + "] " + wb.Driver.LastName + " ";

            comment += wb.Driver.FirstName == null ? "" : " " + wb.Driver.FirstName[0] + ".";
            comment += wb.Driver.MiddleName == null ? "" : " " + wb.Driver.MiddleName[0] + ".";

            comment += " c " + wb.FromDate.ToString("DD.MM.YYYY") + " по " + wb.ToDate.ToString("DD.MM.YYYY");

            report.Id = counter++;
            report.Timestamp = wb.Timestamp;
            report.Name = "Путевой лист";
            report.Comment = comment;
            report.Mileage = wb.EndMileage - wb.StartMileage;

            return report;
        }
    }
}

