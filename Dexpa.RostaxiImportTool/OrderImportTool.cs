using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Factories;
using Dexpa.Core.Model;
using Dexpa.Infrastructure.Repositories;

namespace Dexpa.RostaxiImportTool
{
    class OrderImportTool : ImportToolBase
    {
        public OrderImportTool(string filename, DbContext context)
            : base(filename, context)
        {
        }

        protected override void ImportData()
        {
            var factory = new OrderFactory();
            var driverRepository = new DriverRepository(mDbContext);
            var customerRepository = new CustomerRepository(mDbContext);
            var orderRepository = new OrderRepository(mDbContext);
            //#	Дата	System.Windows.Controls.TextBlock	Телефон	Район	Подача	Куда ехать	Время	Поз	Тариф	Оплата	Сумма	Принят	На месте	Отзвон	System.Windows.Controls.Image
            using (var streamRead = new StreamReader(mFileName))
            {
                var header = streamRead.ReadLine();
                while (!streamRead.EndOfStream)
                {
                    var line = streamRead.ReadLine();
                    var values = line.Split('\t');
                    var date = values[1];
                    var time = values[7];
                    var phoneString = values[3];
                    var fromAddress = values[5];
                    var toAddress = values[6];
                    var callsign = values[8];
                    var cost = double.Parse(values[11], CultureInfo.InvariantCulture);

                    var dateTime = DateTime.Parse(date + " " + time, CultureInfo.GetCultureInfo("ru"));

                    Customer customer = null;
                    customer = customerRepository.Single(c => c.Phone == phoneString);
                    if (customer == null)
                    {
                        customer = CustomerFactory.CreateCustomer(string.Empty, phoneString);
                        customerRepository.Add(customer);
                        customer = customerRepository.Single(c => c.Phone == phoneString);
                    }
                    // Driver driver = driverRepository.Single(d => d.Callsign == callsign);

                    // var order = factory.CreateOrder(customer, fromAddress, toAddress, dateTime, driver, cost);

                    // orderRepository.Add(order);
                    RecordsImported++;
                }
            }
        }
    }
}
