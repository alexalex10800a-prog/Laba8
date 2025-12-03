using DAL8.Interfaces;
using DAL8.Entities;
using DAL8.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DAL8.Repositories
{
    public class ReportRepositorySQL : IReportsRepository
    {
        private Model1 db;

        public ReportRepositorySQL(Model1 dbcontext)
        {
            this.db = dbcontext;
        }

        public List<EmployeeProjectsReport> GetEmployeesWithProjectsByDepartment(int departmentId)
        {
            return db.Database.SqlQuery<EmployeeProjectsReport>(
                "EXEC mydb.GetEmployeesWithProjectsByDepartment @DepartmentId",
                new SqlParameter("@DepartmentId", departmentId))
                .ToList();
        }
    }
}