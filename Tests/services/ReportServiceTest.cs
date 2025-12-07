using BLL8.Services;
using BLL8.models;
using BLL8;
using DAL8;
using DAL8.Interfaces;
using DAL8.Entities;
using Moq;
using Tests.Mocks;


namespace Tests
{
    [TestFixture]
    public class EmployeeReportServiceTest
    {
        private Mock<IDbRepos> _uowMock;
        private DBDataOperations _businessService;
        private ReportServiceForLab4 _reportService;

        [SetUp]
        public void Setup()
        {
            _uowMock = MockUnitOfWork.GetMock();
            _businessService = new DBDataOperations(_uowMock.Object);
            _reportService = new ReportServiceForLab4(_uowMock.Object);
        }

        // ====================
        // ТЕСТЫ БИЗНЕС-ЛОГИКИ (CreateEmployee)
        // ====================

        [Test]
        public void CreateEmployee_Success_WhenDepartmentHasLessThan3Specialists()
        {
            // Arrange - Добавляем только 2 сотрудников (лимит 3)
            // Убедимся, что в отделе 1 и специальности 1 уже есть 2 сотрудника
            MockEmployeeRepository.employees.Clear();
            for (int i = 1; i <= 2; i++)
            {
                MockEmployeeRepository.employees.Add(new employee
                {
                    employee_id = i,
                    full_name = $"Существующий сотрудник {i}",
                    department_code_FK2 = 1,  // IT отдел
                    specialty_code_FK1 = 1     // Программист
                });
            }

            var newEmployee = new EmployeeDTO
            {
                FullName = "Новый Сотрудник",
                DepartmentCode = 1,
                SpecialtyCode = 1
            };

            // Act
            var result = _businessService.CreateEmployee(newEmployee);

            // Assert
            Assert.IsTrue(result.IsSuccess, "Должен успешно создать сотрудника");
            Assert.That(result.Message, Contains.Substring("успешно"));
            Assert.AreEqual(3, MockEmployeeRepository.employees.Count, "Должно быть 3 сотрудника в отделе");
        }

        [Test]
        public void CreateEmployee_Fail_WhenDepartmentHas3Specialists()
        {
            // Arrange - Добавляем 3 сотрудников (достигаем лимита)
            MockEmployeeRepository.employees.Clear();
            for (int i = 1; i <= 3; i++)
            {
                MockEmployeeRepository.employees.Add(new employee
                {
                    employee_id = i,
                    full_name = $"Существующий сотрудник {i}",
                    department_code_FK2 = 1,  // IT отдел
                    specialty_code_FK1 = 1     // Программист
                });
            }

            var newEmployee = new EmployeeDTO
            {
                FullName = "Четвертый Сотрудник",
                DepartmentCode = 1,
                SpecialtyCode = 1
            };

            // Act
            var result = _businessService.CreateEmployee(newEmployee);

            // Assert
            Assert.IsFalse(result.IsSuccess, "Должен вернуть ошибку при превышении лимита");
            Assert.That(result.Message, Contains.Substring("Максимум 3 разрешено")); 
            Assert.AreEqual(3, MockEmployeeRepository.employees.Count, "Не должно добавить четвертого сотрудника");
        }

        // ====================
        // ТЕСТЫ ОТЧЕТОВ (ReportServiceForLab4)
        // ====================

        [Test]
        public void GetEmployeesWithProjectsByDepartment_Success_ReturnsCorrectData()
        {
            // Arrange
            int departmentId = 1; // IT отдел
            string expectedDepartmentName = "IT отдел";

            // Act
            var result = _reportService.GetEmployeesWithProjectsByDepartment(departmentId);

            // Assert
            Assert.IsNotNull(result, "Не должен возвращать null");
            Assert.Greater(result.Count, 0, "Должен вернуть хотя бы одного сотрудника");

            // Проверяем, что все сотрудники из нужного отдела
            foreach (var employee in result)
            {
                Assert.AreEqual(expectedDepartmentName, employee.DepartmentName,
                    $"Сотрудник {employee.FullName} должен быть из отдела {expectedDepartmentName}");
            }

            // Проверяем структуру данных
            var firstEmployee = result.First();
            Assert.IsNotNull(firstEmployee.FullName, "Должно быть указано ФИО");
            Assert.IsNotNull(firstEmployee.SpecialtyName, "Должна быть указана специальность");
            Assert.IsNotNull(firstEmployee.ParticipationStatus, "Должен быть указан статус");
        }

        [Test]
        public void GetEmployeesWithProjectsByDepartment_ReturnsCorrectCount()
        {
            // Arrange
            int departmentId = 1; // IT отдел

            // Посчитаем сколько сотрудников должно быть в IT отделе
            var expectedCount = MockEmployeeRepository.employees
                .Count(e => e.department_code_FK2 == departmentId);

            // Act
            var result = _reportService.GetEmployeesWithProjectsByDepartment(departmentId);

            // Assert
            Assert.AreEqual(expectedCount, result.Count,
                $"Должно вернуть {expectedCount} сотрудников из IT отдела");
        }

        [TearDown]
        public void TearDown()
        {
            // Очищаем тестовые данные после каждого теста
            MockEmployeeRepository.employees.Clear();
            // Восстанавливаем исходные данные
            MockEmployeeRepository.employees.AddRange(new List<employee>
            {
                new employee { employee_id = 1, full_name = "Иванов Иван", department_code_FK2 = 1, specialty_code_FK1 = 1 },
                new employee { employee_id = 2, full_name = "Петров Петр", department_code_FK2 = 1, specialty_code_FK1 = 2 },
                new employee { employee_id = 3, full_name = "Сидоров Сидор", department_code_FK2 = 2, specialty_code_FK1 = 1 }
            });
        }
    }
}