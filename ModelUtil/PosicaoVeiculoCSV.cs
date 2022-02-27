using FileHelpers;
using System;

namespace Data
{
    [DelimitedRecord(";")]
    public class PosicaoVeiculoCSV
    {
        [FieldCaption("Id Veiculo")]
        public int IDVEICULO { get; set; }
        [FieldCaption("Placa")]
        public string PLACA { get; set; }
        [FieldCaption("Data")]
        [FieldConverter(ConverterKind.Date, "dd-MM-yyyy")]
        public DateTime DATAPOSICAO { get; set; }
        [FieldCaption("Longitude")]
        public string LATITURE { get; set; }
        [FieldCaption("Latitude")]
        public string LONGITUTE { get; set; }
        [FieldCaption("Endereço")]
        public string ENDERECO { get; set; }
        [FieldCaption("Velocidade")]
        public int VELOCIDADE { get; set; }
        [FieldCaption("Ignição")]
        [FieldConverter(ConverterKind.Boolean,"Ligado","Desligado")]
        public bool IGNICAO { get; set; }
    }
}
