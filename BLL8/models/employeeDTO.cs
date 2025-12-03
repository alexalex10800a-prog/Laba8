using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL8.models;
using DAL8;
namespace BLL8.models
{
    public class EmployeeDTO
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public int SpecialtyCode { get; set; }
        public int DepartmentCode { get; set; }

        public string DepartmentName { get; set; }
        public string SpecialtyName { get; set; }

        public EmployeeDTO() { }
        public EmployeeDTO(employee e)
        {
            FullName = e.full_name;
            SpecialtyCode = e.specialty_code_FK1;
            DepartmentCode = e.department_code_FK2;
            ID = e.employee_id;
        }
    }
}
