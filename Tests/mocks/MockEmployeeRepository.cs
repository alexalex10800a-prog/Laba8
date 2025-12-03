using System.Collections.Generic;
using System.Linq;
using DAL8;
using DAL8.Interfaces;
using Moq;

namespace Tests.Mocks
{
    public static class MockEmployeeRepository
    {
        // Test data
        public static List<employee> employees = new List<employee>()
        {
            new employee()
            {
                employee_id = 1,
                full_name = "Иванов Иван Иванович",
                specialty_code_FK1 = 1,
                department_code_FK2 = 1
            },
            new employee()
            {
                employee_id = 2,
                full_name = "Петров Петр Петрович",
                specialty_code_FK1 = 2,
                department_code_FK2 = 1
            },
            new employee()
            {
                employee_id = 3,
                full_name = "Сидоров Сидор Сидорович",
                specialty_code_FK1 = 1,
                department_code_FK2 = 2
            }
        };

        // Setting up mock repository methods
        public static Mock<IRepository<employee>> GetMock()
        {
            var mock = new Mock<IRepository<employee>>();

            mock.Setup(m => m.GetList()).Returns(() => employees);

            mock.Setup(m => m.GetItem(It.IsAny<int>()))
                .Returns((int id) => employees.FirstOrDefault(o => o.employee_id == id));

            mock.Setup(m => m.Create(It.IsAny<employee>()))
                .Callback((employee emp) =>
                {
                    emp.employee_id = employees.Count > 0 ? employees.Max(e => e.employee_id) + 1 : 1;
                    employees.Add(emp);
                });

            mock.Setup(m => m.Update(It.IsAny<employee>()))
                .Callback((employee emp) =>
                {
                    var existing = employees.FirstOrDefault(e => e.employee_id == emp.employee_id);
                    if (existing != null)
                    {
                        employees.Remove(existing);
                        employees.Add(emp);
                    }
                });

            mock.Setup(m => m.Delete(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    var emp = employees.FirstOrDefault(e => e.employee_id == id);
                    if (emp != null)
                        employees.Remove(emp);
                });

            return mock;
        }
    }
}