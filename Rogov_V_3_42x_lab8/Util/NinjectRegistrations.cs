using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL8;
using BLL8.Interface;
using BLL8.Services;
using Ninject.Modules;
namespace Rogov_V_3_42x_lab8.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbCrud>().To<DBDataOperations>();
            Bind<IReportService>().To<ReportServiceForLab4>();
        }
    }
}
