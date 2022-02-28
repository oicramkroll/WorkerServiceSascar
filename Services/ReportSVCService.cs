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
        private readonly IGenericRepository<Data.PosicaoVeiculo> _repoCar;
        private readonly IUnitOfWork _unitOfWork;
        public ReportSVCService(IGenericRepository<Data.PosicaoVeiculo> repoCar, IUnitOfWork unitOfWork)
        {
            currentDirectory = AppContext.BaseDirectory;
            _repoCar = repoCar;
            _unitOfWork = unitOfWork;
        }
        public void GenerateByDateInterval()
        {

            DateTime startDate = GetInputDate(
                "Data de inicio do relatorio com o fomrato 'AAAA-MM-DD 00:00:00' (Horario opcional)",
                "Data inválida, tente novamente");
            DateTime endDate = GetInputDate(
                "Data de Fim do relatorio com o fomrato 'AAAA-MM-DD 00:00:00' (Horario opcional)",
                "Data inválida, tente novamente");

            Generate(startDate, endDate);
            Console.WriteLine("Precione enter para inicar novamente");
            Console.ReadLine();
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


            var client = new SASCAR.SasIntegraWSClient();
            var cars = client.obterVeiculos(user, pwd, 1000, 0);


            Console.WriteLine($"Preparando para verificar {cars.Count()} veiculos.");
            foreach (var car in cars.ToList())
            {
                Console.WriteLine(" ... ");
                Console.WriteLine($"Recuperar dados do Veiculo, placa: {car.placa}, id:{car.idVeiculo} ");
                //TODO: Criar Listagem para o armazenamento do arquivo aqui
                var listPositionCSV = new List<PosicaoVeiculoCSV>();

                var nDateEnd = start.AddDays(1);
                var nDateStar = start;
                var notSameDate = true;

                //verifica dia a dia se existem registros para o veiculo
                while (notSameDate)
                {
                    nDateEnd = nDateStar.AddDays(1);
                    Console.Write($"De {nDateStar.ToString("yyyy-MM-dd HH:mm:ss")} até {nDateEnd.ToString("yyyy-MM-dd HH:mm:ss")}");
                    if (nDateEnd <= end)
                    {
                        var positions = client.obterPacotePosicaoHistorico(user, pwd,
                            nDateStar.ToString("yyyy-MM-dd HH:mm:ss"),
                            nDateEnd.ToString("yyyy-MM-dd HH:mm:ss"),
                            car.idVeiculo);
                        var listPosition = new List<PosicaoVeiculo>();
                        if (positions != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"{positions.Count()} posições encontradas.");
                            Console.WriteLine($"...");
                            var total = positions.Count();

                            foreach (var pos in positions.ToList())
                            {
                                var index = positions.ToList().FindIndex(x => x == pos);
                                var posExist = _repoCar.getAll().Any(x =>
                                    x.IDVEICULO == pos.idVeiculo &&
                                    x.PLACA == car.placa &&
                                    x.DATAPOSICAO == pos.dataPosicao
                                );

                                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);

                                Console.WriteLine($"Posicao:{index}, Placa:{car.placa}, existnte no banco: {(posExist ? "sim" : "nao")} {(((index * 100) / total) + 1)}%");
                                if (!posExist)
                                {

                                    listPosition.Add(new PosicaoVeiculo
                                    {
                                        IDVEICULO = pos.idVeiculo,
                                        PLACA = car.placa,
                                        DATAPOSICAO = pos.dataPosicao,
                                        ENDERECO = $"UF: {pos.uf}, Cidade: {pos.cidade}, Rua: {pos.rua}",
                                        IGNICAO = pos.ignicao == 1,
                                        LATITURE = pos.latitude.ToString(),
                                        LONGITUTE = pos.longitude.ToString(),
                                        VELOCIDADE = pos.velocidade
                                    });
                                }

                                listPositionCSV.Add(new PosicaoVeiculoCSV
                                {
                                    IDVEICULO = pos.idVeiculo,
                                    PLACA = car.placa,
                                    DATAPOSICAO = pos.dataPosicao,
                                    ENDERECO = $"UF: {pos.uf}, Cidade: {pos.cidade}, Rua: {pos.rua}",
                                    IGNICAO = pos.ignicao == 1,
                                    LATITURE = pos.latitude.ToString(),
                                    LONGITUTE = pos.longitude.ToString(),
                                    VELOCIDADE = pos.velocidade
                                });
                            }

                        }
                        else
                        {
                            Console.WriteLine($" | Sem Registros");
                        }

                        //registrando
                        if (listPosition.Count > 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Registrando no banco de dados...");
                            _repoCar.saveRange(listPosition);
                            _unitOfWork.commit();

                        }
                    }
                    else
                    {
                        notSameDate = false;
                    }
                    nDateStar = nDateStar.AddDays(1);

                }

                try
                {
                    if (!Directory.Exists(dirDestity))
                    {
                        Directory.CreateDirectory(dirDestity);
                    }


                    var totalLinhas = listPositionCSV.Count();
                    var pageSize = 1000;
                    var totalPages = (int)Math.Ceiling(((double)totalLinhas / pageSize));
                    for (int i = 0; i < totalPages; i++)
                    {
                        var skip = (i) * pageSize;
                        var results = listPositionCSV.Skip(skip).Take(pageSize).ToList();

                        var engine = new FileHelperEngine<PosicaoVeiculoCSV>();
                        engine.HeaderText = engine.GetFileHeader();
                        var fileName = $"sasCar_{car.placa}_{nDateStar.ToString("ddMMyyyy")}_{nDateEnd.ToString("ddMMyyyy")}_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{(i + 1)}.csv";
                        Console.WriteLine();
                        Console.WriteLine($"Gerando arquivo {fileName}");
                        engine.WriteFile($"{dirDestity}/{fileName}", results);
                    }

                   
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                //}

                Thread.Sleep(3000);
                Console.Clear();
            }
            Console.WriteLine("Processo finalizado com sucesso");
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
