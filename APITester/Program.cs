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
                //BaseAddress = new Uri("http://localhost:26585/login")
                BaseAddress = new Uri("https://localhost:44346/maintenance/signup")
            };
            var sendObject = new
            {
                /*user_id = 3,
                phone = "1234567890",
                property_id = 1,
                current_address = "124 hello 3st",
                monthly_income = 15.0,
                comment = "pls give me appartment",
                status = "not looked at"*/
                /*property_id = 1,
                user_id = 3,
                manager_id = 1,
                priority = 10,
                description = "dfsdsf boing",
                status = "not fixed"*/
                /*address = "123 hello st",
                rent = 12.53,
                manager_id = 1,
                manager_name = "manager1",
                balance = 10.65,
                details = new
                {
                    sqft = 1002,
                    bedrooms = 3,
                    bathrooms = 2,
                    description = "nice"
                }*/
                /*full_name = "Andrew Frank4",
                username = "afrank4",
                password = "password4",
                is_manager = false*/
                //balance = 10.32
                //property_id = 4
                //status = "fixed"
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
