using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FalunDagisMenyParser
{
    public static class DurableOrchestrator
    {
        [FunctionName("DurableOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {   
            var weekOffset = context.GetInput<int>();

            for(int currentWeekOffset = weekOffset;currentWeekOffset<=5;currentWeekOffset++)
            {
                var menuItems = await context.CallActivityAsync<IEnumerable<MenuItem>>("FetchAndParseMenu", currentWeekOffset);
                await context.CallActivityAsync<string>("StoreParsedMenu", menuItems);
            }
        }
    }
}