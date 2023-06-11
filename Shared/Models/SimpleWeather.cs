namespace MtlsClientCredentitals.Models;

public class SimpleWeather
{
    public static IReadOnlyCollection<SimpleWeather> Fake => GetFakeSimpleWeathers();

    public DateTime Date { get; set; }
    public int CityId { get; set; }
    public int Temperature { get; set; }

    private static SimpleWeather Random => new()
    {
        Date = GetRandomDateTime(),
        CityId = new Random().Next(1, 10),
        Temperature = new Random().Next(-30, 45),
    };

    private static DateTime GetRandomDateTime()
    {
        var random = new Random();

        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2024, 1, 1);
        TimeSpan timeSpan = endDate - startDate;
        TimeSpan newSpan = new(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);
        return startDate + newSpan;
    }

    private static List<SimpleWeather> GetFakeSimpleWeathers()
    {
        var simpleWeathers = new List<SimpleWeather>(10);
        for (int i = 0; i < 10; i++)
        {
            simpleWeathers.Add(Random);
        }

        return simpleWeathers;
    }
}