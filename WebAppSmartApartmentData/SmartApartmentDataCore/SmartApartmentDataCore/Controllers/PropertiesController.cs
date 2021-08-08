using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartApartmentDataCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace SmartApartmentDataCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertiesController : ControllerBase
    {

        private IConfiguration configuration;
      
        [HttpGet]
        public IEnumerable<PropertiesViewModel> Get(string queryParameter, string filter)
        {
            var properties = new List<PropertiesViewModel>();
         
            using (var client = new HttpClient())
            {
                var uriBuilder = GetUriParameters(client, queryParameter, filter);
                var responseTask = client.GetAsync(uriBuilder.Uri);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result);

                    JObject jsonTask = JObject.Parse(readTask.ToString());
                    var hits = jsonTask["hits"]["hits"];

                    foreach (var value in hits)
                    {
                        var source = value["_source"];
                        var property = JsonConvert.DeserializeObject<PropertiesViewModel>(source.ToString());
                        properties.Add(property);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error, please contact administrator");
                }

            }
            var uniqueProperties = GetGroupDistinctData(properties);
            return uniqueProperties;
        }
        /// <summary>
        /// Group by because mgmt file contains duplicated information about name, market and mgmtID
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public IEnumerable<PropertiesViewModel> GetGroupDistinctData(List<PropertiesViewModel> properties)
        {
           
            IEnumerable<PropertiesViewModel> uniqueProperties = properties.GroupBy(c => new
            {
                c.name,
                c.mgmtID,
                c.propertyID
            }).Select(x => x.First());

            return uniqueProperties;
        }

        public PropertiesController(IConfiguration iconfig)
        {
            configuration = iconfig;
        }

        public Uri GetApiProperties()
        {
            var api = new Uri(this.configuration.GetSection("AwsGateway")["ApiGateway"]);
            return api;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">Http request</param>
        /// <param name="queryParameter">query properties or management searched</param>
        /// <param name="filter">market</param>
        /// <returns></returns>
        public UriBuilder GetUriParameters(HttpClient client, string queryParameter, string filter)
        {
            client.BaseAddress = GetApiProperties();

            var uriBuilder = new UriBuilder(client.BaseAddress);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["query"] = '"' + queryParameter + '"';
            query["filter"] = '"' + filter + "'";
            uriBuilder.Query = query.ToString();

            return uriBuilder;
        }


    }
        
}
