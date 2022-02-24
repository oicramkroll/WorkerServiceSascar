using System;
using System.Threading;

namespace ManagerReport
{
    class Program
    {
        private static bool keepRunning = true;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                keepRunning = false;
            };
            Console.WriteLine("Precione Ctrl+c para sair da aplicação");
            while (keepRunning)
            {
                Console.WriteLine("Data de inicio do relatorio com o fomrato 'AAAA-MM-DD 00:00:00'");
                var data = Console.ReadLine();
                Console.WriteLine(data);
                Thread.Sleep(2000);
                Console.Clear();
            }
            Console.WriteLine("Fechando aplicação...");
            
            
        }
    }
}
