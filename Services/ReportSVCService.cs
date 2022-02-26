using Data;
using FileHelpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Services

{
    public class ReportSVCService : IReportSVC
    {
        private readonly string currentDirectory;
        private readonly CarDbContext dbContext;
        public ReportSVCService(CarDbContext context)
        {
            currentDirectory = AppContext.BaseDirectory;
            dbContext = context;
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

            var dirDestity = configs["DiretorioDestino"].ToString();
            var user = configs["UserWS"]["Login"].ToString();
            var pwd = configs["UserWS"]["Password"].ToString();
            var dateStart = start.ToString("yyyy-MM-dd HH:mm:ss");
            var dateEnd = end.ToString("yyyy-MM-dd HH:mm:ss");

            var client = new SASCAR.SasIntegraWSClient();
            
            var cars = client.obterVeiculos(user,pwd,1000,0);
            
            foreach (var item in cars.ToList())
            {
                //TODO: executar consulta de historico de veiculos.
                var positions = client.obterPacotePosicaoHistorico(user, pwd, dateStart, dateEnd, item.idVeiculo);

                foreach (var pos in positions.ToList())
                {
                    //TODO: gerar arquivo para armazenar histórico na pasta de destino informada
                    //no arquivo de configuração com o nome sasCar_yyyyMMddHHmm_1.csv .
                    
                    var engine = new FileHelperEngine<RelatorioPosicoesCSV>();

                    var positionsCSV = new List<RelatorioPosicoesCSV>();


                    

                    //TODO: armazenar no banco mysql o historico do veiculo.
                    dbContext.RelatorioPosicoes.Add(new RelatorioPosicoes() { Id = item.idVeiculo });
                    dbContext.SaveChanges();

                    //TODO: para cada histórico gravado no banco gravar uma linha no arquivo tambem.
                    
                    //TODO: quando o arquivo chegar a 1000 linhas fechar o arquivo
                    //e gerar outro com nome sasCar_yyyyMMddHHmm_[sequencial].csv .
                }
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
