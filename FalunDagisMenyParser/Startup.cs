using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: WebJobsStartup(typeof(FalunDagisMenyParser.Startup))]

namespace FalunDagisMenyParser
{
    public sealed class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services
                .AddHttpClient()
                .AddSingleton(_ => new Settings
                {
                    Url = Environment.GetEnvironmentVariable("MenuUrl"),
                    StorageConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString")
                });
        }
    }
}
