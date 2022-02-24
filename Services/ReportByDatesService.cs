using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Services

{
    public class ReportByDatesService:IReportByDates
    {
        private readonly string currentDirectory;
        public ReportByDatesService()
        {
            currentDirectory = Directory.GetCurrentDirectory();
        }
        public void Generate() {
            Console.WriteLine(currentDirectory);
            DateTime startDate = DateTime.MinValue;
            while (startDate == DateTime.MinValue )
            {
                try
                {
                    Console.WriteLine("Data de inicio do relatorio com o fomrato 'AAAA-MM-DD 00:00:00'");
                    var value = Console.ReadLine();
                    startDate = Convert.ToDateTime(value);

                }
                catch (Exception)
                {
                    Console.WriteLine("Data inválida, tente novamente");
                    startDate = DateTime.MinValue;
                }
            }
            Console.WriteLine("Ok");
            Thread.Sleep(2000);
            Console.Clear();
        }
    }
}
