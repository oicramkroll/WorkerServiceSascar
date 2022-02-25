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
        public void Generate()
        {
            //Console.WriteLine(currentDirectory);
            DateTime startDate = GetInputDate(
                "Data de inicio do relatorio com o fomrato 'AAAA-MM-DD 00:00:00'",
                "Data inválida, tente novamente");
            Console.WriteLine("Ok");
            Thread.Sleep(2000);
            Console.Clear();
        }

        private static DateTime GetInputDate(string msgLine, string mstErro)
        {
            DateTime startDate = DateTime.MinValue;
            while (startDate == DateTime.MinValue)
            {
                try
                {
                    Console.WriteLine(msgLine);
                    var value = Console.ReadLine();
                    startDate = Convert.ToDateTime(value);

                }
                catch (Exception)
                {
                    Console.WriteLine(mstErro);
                    startDate = DateTime.MinValue;
                }
            }

            return startDate;
        }
    }
}
