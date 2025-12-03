using DAL8.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL8.Interfaces
{
    public interface IReportsRepository 
    {
        List<EmployeeProjectsReport> GetEmployeesWithProjectsByDepartment(int departmentId);
    }
}
