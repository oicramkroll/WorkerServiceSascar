using FileHelpers;

namespace Data
{
    [DelimitedRecord(";")]
    public class RelatorioPosicoesCSV
    {
        public int Id;
        public int Dataposicao;
        public int Velocade;
        public int Ignicao;
        public int Rua;
        public int Numero;
        public int Bairro;
        public int Cade;
        public int UF;
        public int Temp1;
        public int Temp2;
        public int Temp3;
        public int GPS;
        public int Latitude;
        public int Longitude;
        public int Descricao;
        public int Horimetro;
        public int Hodometro;
        public int Bateria;
        public int Satelite;
        public int Bloqueio;
        public int Memoria;
        public int CargaBateria;
        public int Ima;
        public int DataGeracaoRelatorio;
        public int Placa;
    }
}
