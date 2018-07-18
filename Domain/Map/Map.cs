using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Map
{
    public class Usuario_map : EntityTypeConfiguration<Usuario>, IMapping
    {
        public Usuario_map()
        {
            this.HasKey(x => new { x.cd_usuario });

            this.Property(x => x.cd_usuario).IsRequired();
            this.Property(x => x.login).IsRequired();
            this.Property(x => x.nome).IsRequired();
            this.Property(x => x.senha).IsRequired();
            this.Property(x => x.email_particular).IsOptional();


        }
    }


    public class Pedido_map : EntityTypeConfiguration<VPedidoVenda>, IMapping
    {
        public Pedido_map()
        {
            this.HasKey(x => new { x.cd_empresa, x.nr_pedido });

            this.Property(x => x.cd_empresa).IsRequired();
            this.Property(x => x.dt_pedido).IsRequired();
            this.Property(x => x.nr_original).IsOptional();
            this.Property(x => x.descricao).IsOptional();
            this.Property(x => x.cd_venda).IsOptional();
            this.Property(x => x.vl_pedido).IsRequired();


        }
    }


    public class PedidoBloqueio_map : EntityTypeConfiguration<PedidoBloqueio>, IMapping
    {
        public PedidoBloqueio_map()
        {
            this.HasKey(x => new { x.cd_empresa, x.nr_pedido });

            this.Property(x => x.cd_empresa).IsRequired();
            this.Property(x => x.nr_pedido).IsRequired();

            this.Property(x => x.cd_tipobloqueio).IsOptional();
            this.Property(x => x.cd_usuario_blo).IsOptional();
            this.Property(x => x.cd_usuario_desblo).IsOptional();
            this.Property(x => x.dt_bloqueio).IsOptional();
            this.Property(x => x.dt_desblo).IsOptional();
            this.Property(x => x.historico).IsOptional();
            this.Property(x => x.id_pedidobloqueio).IsRequired();
            this.Property(x => x.motivo_desblo).IsOptional();
            this.Property(x => x.situacao).IsOptional();


        }
    }

}
