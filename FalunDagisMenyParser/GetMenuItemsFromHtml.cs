using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Diagnostics;

namespace FalunDagisMenyParser
{
    public partial class GetMenuItemsFromHtml
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly Settings settings;

        public GetMenuItemsFromHtml(IHttpClientFactory clientFactory, Settings settings)
        {
            httpClientFactory = clientFactory;
            this.settings = settings;
        }

        [FunctionName("GetMenuItemsFromHtml")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, 
            [OrchestrationClient]DurableOrchestrationClient starter)
        {

            req.GetQueryParameterDictionary().TryGetValue("offset", out string weekOffsetString);

            int.TryParse(weekOffsetString, out var weekOffset);

            // Function input comes from the request content.
            var instanceId = await starter.StartNewAsync("DurableOrchestrator", weekOffset);

            return new OkObjectResult(instanceId);    
        }

        [FunctionName("FetchAndParseMenu")]
        public async Task<IList<MenuItem>> FetchAndParseMenu([ActivityTrigger] int weekOffset)
        {
            var doc = await FetchHtmlDocument(weekOffset);

            var weekNr = doc.GetWeekNumber();

            return doc.GetMenuItemNodes().ToMenuItems(weekNr);
        }


        public async Task<HtmlDocument> FetchHtmlDocument(int weekOffset)
        {
            var httpClient = httpClientFactory.CreateClient();

            var html = await httpClient.GetStringAsync(settings.GenerateUrl(weekOffset));

            return html.ToHtmlDocument();
        }

        [FunctionName("StoreParsedMenu")]
        public async Task StoreParsedMenuToTableStorage([ActivityTrigger] IList<MenuItem> menuItems)
        {
            var storageAccount = CloudStorageAccount.Parse(settings.StorageConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("daymenu");

            await table.CreateIfNotExistsAsync();

            var batchInsert = new TableBatchOperation();
            menuItems.ToList().ForEach(item => batchInsert.InsertOrReplace(item.ToEntity()));
            
            await table.ExecuteBatchAsync(batchInsert);
        }

    }
}
