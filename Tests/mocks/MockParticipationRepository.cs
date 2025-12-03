using DAL8;
using DAL8.Interfaces;
using Moq;

public static class MockParticipationRepository
{
    public static List<participation> participations = new List<participation>()
    {
        new participation()
        {
            id = 1,
            status = "Активен",
            employee_id_FK1 = 1,
            project_code_FK2 = 101
        },
        new participation()
        {
            id = 2,
            status = "Завершен",
            employee_id_FK1 = 2,
            project_code_FK2 = 101
        }
    };

    public static Mock<IRepository<participation>> GetMock()
    {
        var mock = new Mock<IRepository<participation>>();

        mock.Setup(m => m.GetList()).Returns(() => participations);

        mock.Setup(m => m.GetItem(It.IsAny<int>()))
            .Returns((int id) => participations.FirstOrDefault(o => o.id == id));

        mock.Setup(m => m.Create(It.IsAny<participation>()))
            .Callback((participation part) =>
            {
                part.id = participations.Count > 0 ? participations.Max(p => p.id) + 1 : 1;
                participations.Add(part);
            });

        mock.Setup(m => m.Update(It.IsAny<participation>()))
            .Callback((participation part) =>
            {
                var existing = participations.FirstOrDefault(p => p.id == part.id);
                if (existing != null)
                {
                    participations.Remove(existing);
                    participations.Add(part);
                }
            });

        mock.Setup(m => m.Delete(It.IsAny<int>()))
            .Callback((int id) =>
            {
                var part = participations.FirstOrDefault(p => p.id == id);
                if (part != null)
                    participations.Remove(part);
            });

        return mock;
    }
}