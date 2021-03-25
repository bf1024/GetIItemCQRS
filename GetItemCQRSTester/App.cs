using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GetItemCQRSTester
{
    public class App
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<App> _logger;
        private readonly IConfigurationRoot _configurationRoot;
        public App(IHttpClientFactory clientFactory, ILogger<App> logger, IConfigurationRoot configurationRoot)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configurationRoot = configurationRoot;
        }

        public void Run()
        {
            _logger.LogWarning("Press any button...");
            System.Console.ReadKey();

            var client = _clientFactory.CreateClient("CQRS");
            
            var dataset = GenerateRandomData();

            _logger.LogWarning("Testing post requests...");
            System.Console.ReadKey();
            
            PostTestAsync(dataset, client);
            _logger.LogWarning("Testing get requests...");
            System.Console.ReadKey();
            GetTestAsync(dataset, client);
            System.Console.ReadKey();
            _logger.LogWarning("End of tests.");
            System.Console.ReadKey();
        }

        private Dictionary<string, string> GenerateRandomData()
        {
            var dict = new Dictionary<string, string>();
            for(int i = 0; i < _configurationRoot.GetValue<int>("iterations"); i++)
            {
                dict.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }
            return dict;
        }

        private void PostTestAsync(Dictionary<string, string> dict, HttpClient client)
        {
            IEnumerable<Action> postTasksQuery = dict.Select(kv =>
                ProcessPostUrlAsync(kv.Key, kv.Value, client));

            Action[] tasks = postTasksQuery.ToArray();
            TestRequests(tasks);
           
        }

        private void GetTestAsync(Dictionary<string, string> dict, HttpClient client)
        {
            IEnumerable<Action> getTasksQuery = dict.Select(kv =>
                ProcessGetUrlAsync(kv.Key, client));

            Action[] tasks = getTasksQuery.ToArray();

            TestRequests(tasks);
        }

        private void TestRequests(Action[] tasks)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Parallel.Invoke(tasks);
            stopwatch.Stop();
            _logger.LogError($"Request count:  {tasks.Length:#####}");
            _logger.LogError($"In time:          {stopwatch.ElapsedMilliseconds / 1000} s");
            _logger.LogError($"Requests per second:          {tasks.Length / (stopwatch.ElapsedMilliseconds / 1000)}");
            Console.ReadKey();
        }

        private Action ProcessPostUrlAsync(string Id, string Name, HttpClient client)
        { 
            Action showMethod = () =>
            {
                var content = client.PostAsync($"api/Items/AddItem/{Id}"
                    , new StringContent($"\"{Name}\""
                        , Encoding.UTF8, "application/json")).Result;

                _logger.LogWarning(
                    $"Processed request with id {Id} and name {Name} with status code {content.StatusCode}");
            };

            return showMethod;
        }
    

        private Action ProcessGetUrlAsync(string Id, HttpClient client)
        {
            Action showMethod = () =>
            {
                var content = client.GetAsync($"api/Items/GetItem/{Id}");
                _logger.LogWarning($"Processed request with id {Id}");
            };
            return showMethod;
        }
}
}
