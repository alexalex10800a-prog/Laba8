using DAL8;
using DAL8.Interfaces;
using Moq;

public static class MockSpecialtyRepository
{
    public static List<specialty> specialties = new List<specialty>()
    {
        new specialty()
        {
            specialty_code = 1,
            specialty_name = "Программист"
        },
        new specialty()
        {
            specialty_code = 2,
            specialty_name = "Тестировщик"
        },
        new specialty()
        {
            specialty_code = 3,
            specialty_name = "Аналитик"
        }
    };

    public static Mock<IRepository<specialty>> GetMock()
    {
        var mock = new Mock<IRepository<specialty>>();

        mock.Setup(m => m.GetList()).Returns(() => specialties);

        mock.Setup(m => m.GetItem(It.IsAny<int>()))
            .Returns((int id) => specialties.FirstOrDefault(o => o.specialty_code == id));

        mock.Setup(m => m.Create(It.IsAny<specialty>()))
            .Callback((specialty spec) =>
            {
                spec.specialty_code = specialties.Count > 0 ? specialties.Max(s => s.specialty_code) + 1 : 1;
                specialties.Add(spec);
            });

        mock.Setup(m => m.Update(It.IsAny<specialty>()))
            .Callback((specialty spec) =>
            {
                var existing = specialties.FirstOrDefault(s => s.specialty_code == spec.specialty_code);
                if (existing != null)
                {
                    specialties.Remove(existing);
                    specialties.Add(spec);
                }
            });

        mock.Setup(m => m.Delete(It.IsAny<int>()))
            .Callback((int id) =>
            {
                var spec = specialties.FirstOrDefault(s => s.specialty_code == id);
                if (spec != null)
                    specialties.Remove(spec);
            });

        return mock;
    }
}