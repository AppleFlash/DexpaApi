using System.Data.Entity;
using System.IO;
using Dexpa.Core.Factories;
using Dexpa.Infrastructure.Repositories;

namespace Dexpa.RostaxiImportTool
{
    class DriverImportTool : ImportToolBase
    {
        public DriverImportTool(string filename, DbContext context)
            : base(filename, context)
        {
        }

        protected override void ImportData()
        {
            var factory = new DriverFactory();
            var driverRepository = new DriverRepository(mDbContext);
            //Позыв.	Смена	ФИО	Телефоны	Версия	Условия работы	Автомобиль	Цвет	Гос. номер	Статус	Аренда	Лимит	Дата
            using (var streamRead = new StreamReader(mFileName))
            {
                var header = streamRead.ReadLine();
                while (!streamRead.EndOfStream)
                {
                    var line = streamRead.ReadLine();
                    var values = line.Split('\t');
                    var nameParts = values[2].Split(' ');
                    var lastName = nameParts[0];
                    var firstName = nameParts.Length > 1 ? nameParts[1] : null;
                    var middleName = nameParts.Length > 2 ? nameParts[2] : null;
                    var phone = long.Parse(values[3]);
                    var callsign = values[0];

                    var driver = factory.Create(firstName, lastName, middleName, callsign, phone);
                    driverRepository.Add(driver);
                    RecordsImported++;
                }
            }
        }
    }
}
