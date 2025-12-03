using BLL8.Services;
using BLL8.models;
using BLL8;
using DAL8;
using DAL8.Interfaces;
using DAL8.Entities;
using Moq;
using Tests.Mocks;

[TestFixture]
public class EmployeeReportServiceTest
{
    private Mock<IDbRepos> _uowMock;
    private ReportServiceForLab4 _service;

    // Test data for success cases
    private static readonly int _validDepartmentId = 1;

    // Test data for failure cases
    private static readonly int _invalidDepartmentId = -1;
    private static readonly int _nonExistentDepartmentId = 999;
    private static readonly int _zeroDepartmentId = 0;

    [SetUp]
    public void Setup()
    {
        _uowMock = MockUnitOfWork.GetMock();
        _service = new ReportServiceForLab4(_uowMock.Object);
    }

    // SUCCESS TESTS
    [Test]
    public void GetEmployeesWithProjectsByDepartment_Success_ValidDepartment()
    {
        // Act
        var result = _service.GetEmployeesWithProjectsByDepartment(_validDepartmentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<List<EmployeeProjectsReport>>(result);
        Assert.Greater(result.Count, 0, "Should return at least one employee");

        // Verify data integrity
        foreach (var employee in result)
        {
            Assert.IsNotNull(employee.FullName);
            Assert.IsNotNull(employee.DepartmentName);
            Assert.IsNotNull(employee.SpecialtyName);
            Assert.IsNotNull(employee.ParticipationStatus);
        }
    }

    [Test]
    public void GetEmployeesWithProjectsByDepartment_Success_EmptyDepartment()
    {
        // Arrange - Create a new department with no employees
        var emptyDepartment = new department
        {
            department_code = 100,
            department_name = "Пустой отдел"
        };
        MockDepartmentRepository.departments.Add(emptyDepartment);

        // Act
        var result = _service.GetEmployeesWithProjectsByDepartment(100);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result, "Should return empty list for department with no employees");
    }

    // FAILURE TESTS
    [Test]
    public void GetEmployeesWithProjectsByDepartment_Fail_InvalidDepartmentId()
    {
        // Act & Assert - Test with invalid ID
        var result = _service.GetEmployeesWithProjectsByDepartment(_invalidDepartmentId);
        Assert.IsNotNull(result);
        Assert.IsEmpty(result, "Should return empty list for invalid department ID");
    }

    [Test]
    public void GetEmployeesWithProjectsByDepartment_Fail_NonExistentDepartment()
    {
        // Act & Assert
        var result = _service.GetEmployeesWithProjectsByDepartment(_nonExistentDepartmentId);
        Assert.IsNotNull(result);
        Assert.IsEmpty(result, "Should return empty list for non-existent department");
    }

    [Test]
    public void GetEmployeesWithProjectsByDepartment_Fail_ZeroDepartmentId()
    {
        // Act & Assert
        var result = _service.GetEmployeesWithProjectsByDepartment(_zeroDepartmentId);
        Assert.IsNotNull(result);
        Assert.IsEmpty(result, "Should return empty list for department ID 0");
    }

    [Test]
    public void GetEmployeesWithProjectsByDepartment_Fail_NullDataInMock()
    {
        // Arrange - Corrupt the mock data
        MockEmployeeRepository.employees.Add(new employee
        {
            employee_id = 999,
            full_name = null, // Invalid: null name
            department_code_FK2 = 1,
            specialty_code_FK1 = 1
        });

        // Act
        var result = _service.GetEmployeesWithProjectsByDepartment(1);

        // Assert - Service should handle null data gracefully
        Assert.IsNotNull(result);
        // The test passes if no exception is thrown
    }

    // TEST FOR YOUR BUSINESS LOGIC SERVICE (DBDataOperations)
    [Test]
    public void CreateEmployee_Fail_InvalidData()
    {
        // Arrange - Using the business logic service
        var businessService = new DBDataOperations(_uowMock.Object);

        // Test DTOs with invalid data
        var invalidEmployee1 = new EmployeeDTO
        {
            FullName = "", // Empty name
            DepartmentCode = 1,
            SpecialtyCode = 1
        };

        var invalidEmployee2 = new EmployeeDTO
        {
            FullName = "Valid Name",
            DepartmentCode = -1, // Invalid department
            SpecialtyCode = 1
        };

        var invalidEmployee3 = new EmployeeDTO
        {
            FullName = "Valid Name",
            DepartmentCode = 1,
            SpecialtyCode = 0 // Invalid specialty
        };

        // Act & Assert - All should fail
        var result1 = businessService.CreateEmployee(invalidEmployee1);
        Assert.IsFalse(result1.IsSuccess);

        var result2 = businessService.CreateEmployee(invalidEmployee2);
        Assert.IsFalse(result2.IsSuccess);

        var result3 = businessService.CreateEmployee(invalidEmployee3);
        Assert.IsFalse(result3.IsSuccess);
    }

    [Test]
    public void CreateEmployee_Success_ValidData()
    {
        // Arrange
        var businessService = new DBDataOperations(_uowMock.Object);
        var validEmployee = new EmployeeDTO
        {
            FullName = "Новый Валидный Сотрудник",
            DepartmentCode = 1,
            SpecialtyCode = 2
        };

        // Act
        var result = businessService.CreateEmployee(validEmployee);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Message, Contains.Substring("успешно"));
    }

    // DON'T TEST STORED PROCEDURE REPORTS (as per instructions)
    [Test]
    public void StoredProcedureReport_NotTested()
    {
        // According to instructions: "Так как один из отчетов использует ХП БД, мы его не тестируем"
        // If you have a report that uses stored procedures, skip testing it
        Assert.Pass("Stored procedure reports are not tested (database logic)");
    }
}