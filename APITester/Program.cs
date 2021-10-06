using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APITester
{
    class Program
    {
        static async Task Main(string[] args)
        {

            //README FOR BACKEND

            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:26585/login")
            };
            var sendObject = new
            {
                /*user_id = 3,
                property_id = 1,
                current_address = "124 hello st",
                monthly_income = 10.0,
                comment = "pls give me appartment",
                status = "not looked at"*/
                /*property_id = 1,
                priority = 1,
                description = "leaking roof",
                status = "not fixed",*/
                /*address = "123 hello st",
                rent = 10.0,
                renter = 3,
                manager = 2,
                balance = 0.0,*/
                //full_name = "Andrew Frank",
                username = "afrank",
                password = "password",
                //is_manager = true
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(),Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            var message = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status code:{(int)response.StatusCode} {response.StatusCode}\nBody:{message}");
        }
    }
}
