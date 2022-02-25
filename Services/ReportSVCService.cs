using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Services

{
    public class ReportSVCService:IReportSVC
    {
        private readonly string currentDirectory;
        public ReportSVCService()
        {
            currentDirectory = AppContext.BaseDirectory;
        }
        public void GenerateByDateInterval()
        {
            
            DateTime startDate = GetInputDate(
                "Data de inicio do relatorio com o fomrato 'AAAA-MM-DD 00:00:00'",
                "Data inválida, tente novamente");
            DateTime endDate = GetInputDate(
                "Data de Fim do relatorio com o fomrato 'AAAA-MM-DD 00:00:00'",
                "Data inválida, tente novamente");

            Generate(startDate, endDate);
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

        public void Generate(DateTime start, DateTime end)
        {
            var configs = GetConfigApp();
            var client = new SASCAR.SasIntegraWSClient();
            var cars = client.obterVeiculos(
                configs["UserWS"]["Login"].ToString(),
                configs["UserWS"]["Password"].ToString(),
                1000,
                0
                );
            foreach (var item in cars.ToList())
            {
            //TODO: executar consulta de historico de veiculos.
            //TODO: gerar arquivo para armazenar histórico na pasta de destino informada no arquivo de configuração com o nome sasCar_yyyyMMddHHmm_1.csv .
            //TODO: armazenar no banco mysql o historico do veiculo.
            //TODO: para cada histórico gravado no banco gravar uma linha no arquivo tambem.
            //TODO: quando o arquivo chegar a 1000 linhas fechar o arquivo e gerar outro com nome sasCar_yyyyMMddHHmm_[sequencial].csv .


            }


            Console.WriteLine("Relátorio Gerado com sucesso");
        }

        public JObject GetConfigApp()
        {
            JObject config;
            using (StreamReader r = new StreamReader($"{currentDirectory}/Config.json"))
            {
                string json = r.ReadToEnd();
                config = JObject.Parse(json);
            }
            return config;
        }
    }
}
