﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Group_Assignment_6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<int>> Get()
        {
            HttpClient client = new HttpClient();
            string result;

            try
            {
                HttpResponseMessage response = client.GetAsync("https://www.7timer.info/bin/astro.php?lon=113.2&lat=23.1&ac=0&unit=metric&output=json&tzshift=0").Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
            } catch (Exception ex) {
                return BadRequest(ex);
            }

            try
            {
                ForcastData data = JsonConvert.DeserializeObject<ForcastData>(result);

                List<int> list = new List<int>();

                foreach (var i in data.DataSeries)
                {
                    list.Add(i.Temp2m);
                }

                list.Sort();

                return Ok(list);
            } catch (Exception ex)
            {
                return StatusCode(502, ex);
            }
        }
    }

    public class ForcastData
    {
        public string? Product { get; set; }
        public string? Init { get; set; }

        public List<Forcast> DataSeries { get; set; }
    }

    public class Forcast
    {
        public int TimePoint { get; set; }
        public int CloudCover { get; set; }
        public int Transparency { get; set; }
        public int Lifted_Index { get; set; }
        public int RH2m { get; set; }
        public Wind? Wind10m { get; set; }
        public int Temp2m { get; set; }
        public string? Prec_Type { get; set; }
    }

    public class Wind
    {
        public string? Direction { get; set;}
        public int Speed { get; set;}
    }
}
