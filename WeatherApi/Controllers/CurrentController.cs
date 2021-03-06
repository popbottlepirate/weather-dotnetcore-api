using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weather.Core.Models;
using Weather.OpenWeatherMap;

namespace WeatherApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CurrentController : ControllerBase
    {
        private readonly Client _client;
        public CurrentController(Client client)
        {
            this._client = client;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<WeatherResult>> Get(string lat, string lon)
        {
            if (Double.TryParse(lat, out double latitude) && Double.TryParse(lon, out double longitude))
            {
                try
                {
                    var weatherResponse = await _client.Current(new Coord { Lat = latitude, Lon = longitude });
                    return new WeatherResult { Date = DateTimeOffset.FromUnixTimeSeconds(weatherResponse.Dt).DateTime, Temp = weatherResponse.Main.Temp, Icon = weatherResponse.Weather[0].Icon };
                }
                catch
                {
                    return StatusCode(500, "Error retrieving weather information");
                }
            }
            return BadRequest("Coordinates were missing or incorrect");
            //return await _client.Current(new Coord { lat=42.3305216, lon=-83.0464 });
        }

    }
}