using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL8.Interfaces;
using DAL8.Repositories;
using Ninject.Modules;

namespace BLL8
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule (string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IDbRepos>().To<DbReposSQL>().InSingletonScope().WithConstructorArgument(connectionString);
        }
    }
}
