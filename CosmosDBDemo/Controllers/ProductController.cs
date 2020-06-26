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
            var url = "https://230a0264-0ee0-4-231-b9ee.documents.azure.com:443/";
            var db = "Ecommerce";
            var key = "qJx4Msuv1yt6ie8ro5SytVbSYYEnN1rKbWXrYj9raWc3ckoKZ6spZNjiv2VBo0SSVhh8tExYWpovMikjpwzruA==";
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
            var url = "https://230a0264-0ee0-4-231-b9ee.documents.azure.com:443/";
            var db = "Ecommerce";
            var key = "qJx4Msuv1yt6ie8ro5SytVbSYYEnN1rKbWXrYj9raWc3ckoKZ6spZNjiv2VBo0SSVhh8tExYWpovMikjpwzruA==";
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
