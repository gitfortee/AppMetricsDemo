// <copyright file="SlaTestController.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class SlaTestController : Controller
    {
        private static readonly Random Rnd = new Random();
        private readonly IMetrics _metrics;

        public SlaTestController(IMetrics metrics) { _metrics = metrics; }

        [HttpGet]
        [Route("timer")]
        public async Task<IActionResult> GetTimer()
        {
            using (_metrics.Measure.Timer.Time(SandboxMetricsRegistry.DatabaseTimer, new MetricTags("client_id", "test-client-id")))
            {
                await Task.Delay(Rnd.Next(350), HttpContext.RequestAborted);
            }

            return Ok();
        }
    }
}