using DAL8.Interfaces;
using DAL8;
using DAL8.Interfaces;
using Moq;

namespace Tests.Mocks
{
    public static class MockUnitOfWork
    {
        public static Mock<IDbRepos> GetMock()
        {
            var mock = new Mock<IDbRepos>();

            // Setup all repository properties using the individual mock repositories
            mock.Setup(m => m.Employees).Returns(MockEmployeeRepository.GetMock().Object);
            mock.Setup(m => m.Departments).Returns(MockDepartmentRepository.GetMock().Object);
            mock.Setup(m => m.Specialties).Returns(MockSpecialtyRepository.GetMock().Object);
            mock.Setup(m => m.Projects).Returns(MockProjectRepository.GetMock().Object);
            mock.Setup(m => m.Participations).Returns(MockParticipationRepository.GetMock().Object);
            mock.Setup(m => m.Reports).Returns(MockReportsRepository.GetMock().Object);

            // Setup Save method
            mock.Setup(m => m.Save()).Returns(1);

            return mock;
        }

        // Alternative: Create a concrete test UOW for more control
        public class TestUnitOfWork : IDbRepos
        {
            public IRepository<employee> Employees { get; }
            public IRepository<department> Departments { get; }
            public IRepository<specialty> Specialties { get; }
            public IRepository<project> Projects { get; }
            public IRepository<contract> Contracts { get; }
            public IRepository<participation> Participations { get; }
            public IReportsRepository Reports { get; }

            private int _saveCount = 0;

            public TestUnitOfWork()
            {
                Employees = MockEmployeeRepository.GetMock().Object;
                Departments = MockDepartmentRepository.GetMock().Object;
                Specialties = MockSpecialtyRepository.GetMock().Object;
                Projects = MockProjectRepository.GetMock().Object;
                Participations = MockParticipationRepository.GetMock().Object;
                Reports = MockReportsRepository.GetMock().Object;
            }

            public int Save()
            {
                _saveCount++;
                return 1; // Simulate 1 record saved
            }

            public int GetSaveCount() => _saveCount;

            public void Dispose()
            {
                // Cleanup if needed
            }
        }
    }
}