namespace Shared.Models;

public class Weather
{
    public DateTime Date { get; set; }
    public string City { get; set; } = default!;
    public int Temperature { get; set; }
}
