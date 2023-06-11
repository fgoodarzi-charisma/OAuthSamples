namespace MtlsClientCredentitals.Models;

public class City
{
    public static readonly IEnumerable<City> All = new List<City>
    {
        new City { Id = 1, Name = "Tehran" },
        new City { Id = 2, Name = "Isfahan" },
        new City { Id = 3, Name = "Shiraz" },
        new City { Id = 4, Name = "Tabriz" },
        new City { Id = 5, Name = "Borujerd" },
        new City { Id = 6, Name = "Abadan" },
        new City { Id = 7, Name = "Babol" },
        new City { Id = 8, Name = "Sari" },
        new City { Id = 9, Name = "Rasht" },
        new City { Id = 10, Name = "Ahvaz" },
    };

    public int Id { get; set; }
    public string Name { get; set; } = default!;
}
