using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Collections.Generic;

namespace Cip.Controllers;

[ApiController]
[Route("[controller]")]
public class GeographyController : ControllerBase
{
    [HttpGet("/city")]
    public IEnumerable<City> Get()
    {
        return City.All;
    }
}
