using DAL8;
using DAL8.Interfaces;
using Moq;

public static class MockDepartmentRepository
{
    public static List<department> departments = new List<department>()
    {
        new department()
        {
            department_code = 1,
            department_name = "IT отдел"
        },
        new department()
        {
            department_code = 2,
            department_name = "Бухгалтерия"
        },
        new department()
        {
            department_code = 3,
            department_name = "Отдел кадров"
        }
    };

    public static Mock<IRepository<department>> GetMock()
    {
        var mock = new Mock<IRepository<department>>();

        mock.Setup(m => m.GetList()).Returns(() => departments);

        mock.Setup(m => m.GetItem(It.IsAny<int>()))
            .Returns((int id) => departments.FirstOrDefault(o => o.department_code == id));

        mock.Setup(m => m.Create(It.IsAny<department>()))
            .Callback((department dept) =>
            {
                dept.department_code = departments.Count > 0 ? departments.Max(d => d.department_code) + 1 : 1;
                departments.Add(dept);
            });

        mock.Setup(m => m.Update(It.IsAny<department>()))
            .Callback((department dept) =>
            {
                var existing = departments.FirstOrDefault(d => d.department_code == dept.department_code);
                if (existing != null)
                {
                    departments.Remove(existing);
                    departments.Add(dept);
                }
            });

        mock.Setup(m => m.Delete(It.IsAny<int>()))
            .Callback((int id) =>
            {
                var dept = departments.FirstOrDefault(d => d.department_code == id);
                if (dept != null)
                    departments.Remove(dept);
            });

        return mock;
    }
}