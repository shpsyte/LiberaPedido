using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public partial class Usuario
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 cd_usuario { get; set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string situacao { get; set; }
        public string email_particular { get; set; }

    }


    public partial class VPedidoVenda
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cd_empresa { get; set; }

        public int nr_pedido { get; set; }

        public DateTime dt_pedido { get; set; }
        public string nr_original { get; set; }
        public decimal vl_pedido { get; set; }
        public string cd_venda { get; set; }
        public string descricao { get; set; }
        public string situacao { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    public class GetItensDoc
    {
        public string cd_item { get; set; }
        public string descricao { get; set; }
        public decimal qt_pedida { get; set; }
        public decimal vl_unitario { get; set; }
        public decimal vl_total { get; set; }
    }




    public partial class Pedido_Resposta_Model
    {
        public virtual VPedidoVenda VPedidoVenda { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(255, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string Obs { get; set; }
    }



    public partial class PedidoBloqueio
    {
        public int cd_empresa { get; set; }
        public int nr_pedido { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_pedidobloqueio { get; set; }
        public string historico { get; set; }
        public int cd_usuario_desblo { get; set; }
        public string motivo_desblo { get; set; }
        public DateTime dt_desblo { get; set; }
        public string cd_tipobloqueio { get; set; }
        public DateTime dt_bloqueio { get; set; }
        public int cd_usuario_blo { get; set; }
        public string situacao { get; set; }
    }



}
