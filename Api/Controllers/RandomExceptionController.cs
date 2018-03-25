// <copyright file="RandomExceptionController.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class RandomExceptionController : Controller
    {
        private readonly RandomValuesForTesting _randomValuesForTesting;

        public RandomExceptionController(RandomValuesForTesting randomValuesForTesting)
        {
            _randomValuesForTesting = randomValuesForTesting;
        }

        [HttpGet]
        public void Get()
        {
            throw _randomValuesForTesting.NextException();
        }
    }
}