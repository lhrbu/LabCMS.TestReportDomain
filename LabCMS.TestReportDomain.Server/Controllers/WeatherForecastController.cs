﻿using LabCMS.TestReportDomain.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.Server.Controllers
{
    public class Project
    {
        public string no { get; set; } = "TestDapperNo";
        public string name { get; set; } = "TestDapperName";
        public string name_in_fin { get; set; } = "TestDapperNameInFin";
    }
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DatabaseAccessService _dbService;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _dbService = new(_configuration.GetConnectionString("test"));
        }

        [HttpGet]
        public async ValueTask<IEnumerable<WeatherForecast>> Get()
        {
            Project project = new();
            await _dbService.InsertAsync(project, "projects");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("test")]
        public async ValueTask<IEnumerable<dynamic>> Test()=>
            await _dbService.GetAllAsync( "usage_records");
    }
}
