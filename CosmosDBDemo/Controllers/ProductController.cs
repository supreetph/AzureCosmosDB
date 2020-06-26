using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDBDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CosmosDBDemo.Controllers
{
    public class ProductController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var url = "";
            var db = "Ecommerce";
            var key = "";
            CosmosClient client = new CosmosClient(url, key);
            Database database = await client.CreateDatabaseIfNotExistsAsync(db);
            Container containers = await database.CreateContainerIfNotExistsAsync("Product", "/ProductId");
            var container = client.GetContainer(db,"Product");
            var sqlQueryText = "SELECT * FROM Product ";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Products> queryResultSetIterator = containers. GetItemQueryIterator<Products>(queryDefinition);
           // var response = client.GetDatabaseQueryIterator<Products>(sqlQueryText);
            List<Products> products = new List<Products>();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Products> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Products items in currentResultSet)
                {
                    products.Add(items);
                 
                }
            }

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Products products)
        {
            var url = "";
            var db = "Ecommerce";
            var key = "";
            CosmosClient client = new CosmosClient(url, key);
            Database database = await client.CreateDatabaseIfNotExistsAsync(db);
            Container containers = await database.CreateContainerIfNotExistsAsync("Product", "/ProductId");

            var response = await containers.CreateItemAsync<Products>(products);
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
    }
}
