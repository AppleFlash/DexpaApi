using System.Data.Entity;
using System.IO;
using Dexpa.Core.Factories;
using Dexpa.Infrastructure.Repositories;

namespace Dexpa.RostaxiImportTool
{
    class CustomerImportTool : ImportToolBase
    {
        public CustomerImportTool(string filename, DbContext context)
            : base(filename, context)
        {
        }

        protected override void ImportData()
        {
            var customerRepository = new CustomerRepository(mDbContext);
            //Код	Телефон	Заказы	ФИО	Источник	Адрес подачи	Создан
            using (var streamRead = new StreamReader(mFileName))
            {
                var header = streamRead.ReadLine();
                while (!streamRead.EndOfStream)
                {
                    var line = streamRead.ReadLine();
                    var values = line.Split('\t');
                    var name = values[3];
                    var phone = long.Parse(values[1]);
                    
                    var customer = CustomerFactory.CreateCustomer(name, phone.ToString());
                    customerRepository.Add(customer);
                    RecordsImported++;
                }
            }
        }
    }
}
