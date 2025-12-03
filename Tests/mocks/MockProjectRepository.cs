using DAL8;
using DAL8.Interfaces;
using Moq;

public static class MockProjectRepository
{
    public static List<project> projects = new List<project>()
    {
        new project()
        {
            project_code = 101,
            start_date = DateTime.Now.AddDays(-30),
            end_date = DateTime.Now.AddDays(60),
            cost = 50000,
            contract_code_FK = 1
        },
        new project()
        {
            project_code = 102,
            start_date = DateTime.Now.AddDays(-15),
            end_date = DateTime.Now.AddDays(45),
            cost = 75000,
            contract_code_FK = 2
        }
    };

    public static Mock<IRepository<project>> GetMock()
    {
        var mock = new Mock<IRepository<project>>();

        mock.Setup(m => m.GetList()).Returns(() => projects);

        mock.Setup(m => m.GetItem(It.IsAny<int>()))
            .Returns((int id) => projects.FirstOrDefault(o => o.project_code == id));

        mock.Setup(m => m.Create(It.IsAny<project>()))
            .Callback((project proj) =>
            {
                proj.project_code = projects.Count > 0 ? projects.Max(p => p.project_code) + 1 : 100;
                projects.Add(proj);
            });

        mock.Setup(m => m.Update(It.IsAny<project>()))
            .Callback((project proj) =>
            {
                var existing = projects.FirstOrDefault(p => p.project_code == proj.project_code);
                if (existing != null)
                {
                    projects.Remove(existing);
                    projects.Add(proj);
                }
            });

        mock.Setup(m => m.Delete(It.IsAny<int>()))
            .Callback((int id) =>
            {
                var proj = projects.FirstOrDefault(p => p.project_code == id);
                if (proj != null)
                    projects.Remove(proj);
            });

        return mock;
    }
}