using Api.Helpers;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api
{
    public static class AppBuilderExtensions
    {
        private static readonly HttpClient HttpClient = new HttpClient { BaseAddress = new Uri("http://localhost:52410/api/") };
        private const int SlaEndpointsInterval = 2;
        private const int ApdexSamplesInterval = 2;
        private const int GetEndpointSuccessInterval = 1;
        private const int PutAndPostRequestsInterval = 6;
        private const int RandomSamplesInterval = 10;

        public static IApplicationBuilder UseTestStuff(
            this IApplicationBuilder app,
            IApplicationLifetime lifetime)
        {
            var token = lifetime.ApplicationStopping;

            var slaSamples = new AppMetricsTaskScheduler(
                    TimeSpan.FromSeconds(SlaEndpointsInterval),
                    () => HttpClient.GetAsync("slatest/timer", token));

            slaSamples.Start();

            var apdexSamples = new AppMetricsTaskScheduler(
                    TimeSpan.FromSeconds(ApdexSamplesInterval),
                    () =>
                    {
                        var satisfied = HttpClient.GetAsync("satisfying", token);
                        var tolerating = HttpClient.GetAsync("tolerating", token);
                        var frustrating = HttpClient.GetAsync("frustrating", token);

                        return Task.WhenAll(satisfied, tolerating, frustrating);
                    });

            apdexSamples.Start();

            var randomErrorSamples = new AppMetricsTaskScheduler(
                TimeSpan.FromSeconds(RandomSamplesInterval),
                () =>
                {
                    var randomStatusCode = HttpClient.GetAsync("randomstatuscode", token);
                    var randomException = HttpClient.GetAsync("randomexception", token);

                    return Task.WhenAll(randomStatusCode, randomException);
                });

            randomErrorSamples.Start();

            var testSamples = new AppMetricsTaskScheduler(
                TimeSpan.FromSeconds(GetEndpointSuccessInterval),
                () => HttpClient.GetAsync("test", token));

            testSamples.Start();

            var randomBufferGenerator = new RandomBufferGenerator(50000);
            var postPutSamples = new AppMetricsTaskScheduler(
                TimeSpan.FromSeconds(PutAndPostRequestsInterval),
                () =>
                {
                    var putBytes = new ByteArrayContent(randomBufferGenerator.GenerateBufferFromSeed());
                    var putFormData = new MultipartFormDataContent { { putBytes, "put-file", "rnd-put" } };
                    var putRequest = HttpClient.PutAsync("file", putFormData, token);

                    var postBytes = new ByteArrayContent(randomBufferGenerator.GenerateBufferFromSeed());
                    var postFormData = new MultipartFormDataContent { { postBytes, "post-file", "rnd-post" } };
                    var postRequest = HttpClient.PostAsync("file", postFormData, token);

                    return Task.WhenAll(putRequest, postRequest);
                });

            postPutSamples.Start();

            return app;
        }
    }
}
