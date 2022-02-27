using FileHelpers;
using System;

namespace Data
{
    [DelimitedRecord(";")]
    public class PosicaoVeiculoCSV
    {
        public long ID;
        public int IDVEICULO;
        public string PLACA;
        public DateTime DATAPOSICAO;
        public string LATITURE;
        public string LONGITUTE;
        public string ENDERECO;
        public int VELOCIDADE;
        public bool IGNICAO;
    }
}
