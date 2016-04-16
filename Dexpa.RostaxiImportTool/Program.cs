using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dexpa.Infrastructure;

namespace Dexpa.RostaxiImportTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ModelContext();
            //var filename = "D:\\Repos\\Dexpa\\Data\\drivers.csv";
            //var filename = "D:\\Repos\\Dexpa\\Data\\customers.csv";
            //var filename = "D:\\Repos\\Dexpa\\Data\\orders2.txt";
            var filename = "D:\\Repos\\Dexpa\\Data\\cars.txt";

            Console.WriteLine("Import in porcess");
            var progressMessage = "Items imported: ";
            Console.WriteLine(progressMessage);
            var tool = new CarImportTool(filename, context);
            tool.StartImport();

            while (!tool.Completed)
            {
                Console.SetCursorPosition(progressMessage.Length, 1);
                Console.Write(tool.RecordsImported);
                Thread.Sleep(1000);
            }

            if (tool.Error == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success completed");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Completed with error: {0}", tool.Error.Message);
            }
            Console.ReadLine();
        }
    }
}
