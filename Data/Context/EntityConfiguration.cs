using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public interface IEntityConfiguration
    {
        void AddConfiguration(ConfigurationRegistrar registrar);
    }


    public class ContextConfiguration
    {
        public IEnumerable<IEntityConfiguration> Configurations
        {
            get;
            set;
        }
    }
}
