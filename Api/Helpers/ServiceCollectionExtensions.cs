// <copyright file="ServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Tracking;
using Api.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Api.Helpers
// ReSharper restore CheckNamespace
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestStuff(this IServiceCollection services)
        {
            services.AddTransient<Func<double, RequestDurationForApdexTesting>>(
                serviceProvider => { return apdexTSeconds => new RequestDurationForApdexTesting(apdexTSeconds); });

            services.AddSingleton<RandomValuesForTesting>();

            services.AddTransient(
                serviceProvider =>
                {
                    var optionsAccessor = serviceProvider.GetRequiredService<IOptions<MetricsWebTrackingOptions>>();
                    return new RequestDurationForApdexTesting(optionsAccessor.Value.ApdexTSeconds);
                });

            return services;
        }
    }
}