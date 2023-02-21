using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cip.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Builder<WeatherForecast>.CreateListOfSize(3)
            .All()
            .With(a => a.City = Faker.Address.City())
            .With(a => a.Temperature = Faker.RandomNumber.Next(-10, 40))
            .With(a => a.Date = DateTime.Now.AddDays(1))
            .Build();
    }
}
