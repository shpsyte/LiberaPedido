using Domain;
using Domain.Entity;
using Domain.Map;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class b2yweb_entities : DbContext
    {


        public b2yweb_entities()
            : base("B2yContext")
        {
        }

        public b2yweb_entities(String strEntity = "bavatos")
            : base("name=" + strEntity + "_entities")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.Configuration.LazyLoadingEnabled = false;
            base.Configuration.AutoDetectChangesEnabled = false;



            //Aqui vamos remover a pluralização padrão do Etity Framework que é em inglês
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            /*Desabilitamos o delete em cascata em relacionamentos 1:N evitando
             ter registros filhos     sem registros pai*/
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Basicamente a mesma configuração, porém em relacionamenos N:N
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //Aqui vamos remover a pluralização padrão do Etity Framework que é em inglês
            modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();
            

            //Fazendo o mapeamento com o banco de dados
            //Pega todas as classes que estão implementando a interface IMapping
            ////Assim o Entity Framework é capaz de carregar os mapeamentos
            var typesToMapping = (from x in Assembly.GetExecutingAssembly().GetTypes()
                                  where x.IsClass && typeof(IMapping).IsAssignableFrom(x)
                                  select x).ToList();

            // Varrendo todos os tipos que são mapeamento 
            // Com ajuda do Reflection criamos as instancias 
            // e adicionamos no Entity Framework
            foreach (var mapping in typesToMapping)
            {
                dynamic mappingClass = Activator.CreateInstance(mapping);
                modelBuilder.Configurations.Add(mappingClass);
            }

            modelBuilder.Configurations.Add(new Usuario_map());
            modelBuilder.Configurations.Add(new Pedido_map());
            modelBuilder.Configurations.Add(new PedidoBloqueio_map());

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<VPedidoVenda> Pedidos { get; set; }
        public DbSet<PedidoBloqueio> PedidoBloqueio { get; set; }


    }
}
