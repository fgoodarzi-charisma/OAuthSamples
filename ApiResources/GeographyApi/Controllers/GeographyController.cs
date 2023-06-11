using Microsoft.AspNetCore.Mvc;
using MtlsClientCredentitals.Models;
using System.Collections.Generic;

namespace Geography.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class GeographyController : ControllerBase
{
    [HttpGet("/city")]
    public IEnumerable<City> Get()
    {
        return City.All;
    }
}
