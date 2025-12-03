using DAL8.Entities;
using DAL8.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Mocks
{
    public static class MockReportsRepository
    {
        // Create report data dynamically from other mock repositories
        public static List<EmployeeProjectsReport> GetReportData()
        {
            // Combine data from employee, department, specialty, and participation mocks
            return MockEmployeeRepository.employees.Select(emp =>
            {
                var department = MockDepartmentRepository.departments
                    .FirstOrDefault(d => d.department_code == emp.department_code_FK2);

                var specialty = MockSpecialtyRepository.specialties
                    .FirstOrDefault(s => s.specialty_code == emp.specialty_code_FK1);

                var participation = MockParticipationRepository.participations
                    .FirstOrDefault(p => p.employee_id_FK1 == emp.employee_id);

                var project = participation != null
                    ? MockProjectRepository.projects
                        .FirstOrDefault(p => p.project_code == participation.project_code_FK2)
                    : null;

                return new EmployeeProjectsReport
                {
                    EmployeeId = emp.employee_id,
                    FullName = emp.full_name,
                    DepartmentName = department?.department_name ?? "Не назначен",
                    SpecialtyName = specialty?.specialty_name ?? "Не указана",
                    ProjectCode = project?.project_code ?? 0,
                    ParticipationStatus = participation?.status ?? "не назначен"
                };
            }).ToList();
        }

        public static Mock<IReportsRepository> GetMock()
        {
            var mock = new Mock<IReportsRepository>();

            // Setup the report method using LINQ on mock data
            mock.Setup(m => m.GetEmployeesWithProjectsByDepartment(It.IsAny<int>()))
                .Returns((int departmentId) =>
                {
                    // Filter by department using LINQ
                    return GetReportData()
                        .Where(r =>
                        {
                            // Match department ID with department name
                            var dept = MockDepartmentRepository.departments
                                .FirstOrDefault(d => d.department_code == departmentId);
                            return dept != null && r.DepartmentName == dept.department_name;
                        })
                        .ToList();
                });

            return mock;
        }
    }
}