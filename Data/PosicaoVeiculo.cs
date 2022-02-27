using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    [Table("tb_sascar_posicoes")]
    public class PosicaoVeiculo
    {
        [Key]
        public long ID { get; set; }
        public int IDVEICULO { get; set; }
        public string PLACA { get; set; }
        public DateTime DATAPOSICAO { get; set; }
        public string LATITURE { get; set; }
        public string LONGITUTE { get; set; }
        public string ENDERECO { get; set; }
        public int VELOCIDADE { get; set; }
        public bool IGNICAO { get; set; }
    }
}
