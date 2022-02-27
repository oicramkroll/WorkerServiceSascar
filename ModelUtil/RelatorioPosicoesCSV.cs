using FileHelpers;

namespace Data
{
    [DelimitedRecord(";")]
    public class RelatorioPosicoesCSV
    {
        public int Id;
        public string Dataposicao;
        public decimal Velocade;
        public int Ignicao;
        public string Rua;
        public int Numero;
        public string Bairro;
        public string Cidade;
        public string UF;
        public int Latitude;
        public int Longitude;
        public int Descricao;
        public int Horimetro;
        public int Hodometro;
        public int Bateria;
        public int DataGeracaoRelatorio;
        public int Placa;
    }
}
