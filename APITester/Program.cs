using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APITester
{
    class Program
    {
        private static string baseAddress = "https://localhost:44331/";
        private static int i = 1;
        static async Task Main(string[] args)
        {
            await Tests();
        }

        private static async Task OneTimeTest()
        {
            //README FOR BACKEND

            var client = new HttpClient()
            {
                //BaseAddress = new Uri("http://localhost:26585/login")
                //BaseAddress = new Uri("https://localhost:44346/property/manager/2")
                //BaseAddress = new Uri("https://spikeexercise.azurewebsites.net/property")
                //BaseAddress = new Uri("https://localhost:44331/signup")
                BaseAddress = new Uri("https://movienightapi.azurewebsites.net/signup")
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
                /*address = "123 hello st2",
                rent = 43,
                manager_id = 2,
                renter_id = 1,
                manager_name = "manager3",
                balance = 60.65,
                details = new
                {
                    sqft = 1002,
                    bedrooms = 3,
                    bathrooms = 2,
                    description = "nice"
                }*/
                //full_name = "Andrew Frank2",
                username = "afrank",
                password = "password",
                email = "afrank@testemailtesttest.com",
                //is_manager = false
                //balance = 10.32
                //property_id = 4
                //status = "fixed"
                //renter_id = 2
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            var message = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status code:{(int)response.StatusCode} {response.StatusCode}\nBody:{message}");
        }

        private static async Task Tests()
        {
            Console.WriteLine($"TestSignUp: {await TestSignUp()}");
            Console.WriteLine($"TestSignUp2: {await TestSignUp2()}");
            Console.WriteLine($"TestLogin: {await TestLogin()}");
            Console.WriteLine($"TestLogin2: {await TestLogin2()}");
            Console.WriteLine($"TestAddGroup: {await TestAddGroup()}");
            Console.WriteLine($"TestGetGroup: {await TestGetGroup()}");
            Console.WriteLine($"TestPatchGroup: {await TestPatchGroup()}");
            Console.WriteLine($"TestGetGroup2: {await TestGetGroup2()}");
            Console.WriteLine($"TestGetGroupUsers: {await TestGetGroupUsers()}");
            Console.WriteLine($"TestPatchAlias: {await TestPatchAlias()}");
            Console.WriteLine($"TestGetGroupUsers2: {await TestGetGroupUsers2()}");
            Console.WriteLine($"TestPatchAdmin: {await TestPatchAdmin()}");
            Console.WriteLine($"TestGetGroupUsers3: {await TestGetGroupUsers3()}");
            Console.WriteLine($"TestRemoveUserGroup: {await TestRemoveUserGroup()}");
            Console.WriteLine($"TestGetGroupUsers4: {await TestGetGroupUsers4()}");
            Console.WriteLine($"TestJoinGroup: {await TestJoinGroup()}");
            Console.WriteLine($"TestGetGroupUsers5: {await TestGetGroupUsers5()}");
            Console.WriteLine($"TestDeleteGroup: {await TestDeleteGroup()}");
            Console.WriteLine($"TestGetGroupUsers6: {await TestGetGroupUsers6()}");
            Console.WriteLine($"TestGetGroup3: {await TestGetGroup3()}");
        }

            //Console.WriteLine((int) response.StatusCode);
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

        private static async Task<bool> TestSignUp()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "signup")
            };
            var sendObject = new
            {
                username = $"afrank{i}",
                password = $"password{i}",
                email = $"email@email.com{i}"
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestSignUp2()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "signup")
            };
            var sendObject = new
            {
                username = $"afrank{i}",
                password = $"password{i}",
                email = $"email@email.com{i}"
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestLogin()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "login")
            };
            var sendObject = new
            {
                username = $"afrank{i}",
                password = $"password{i}",
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestLogin2()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "login")
            };
            var sendObject = new
            {
                username = $"afrank45h454g5g45eg45{i}",
                password = $"password{i}",
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestAddGroup()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "group")
            };
            var sendObject = new
            {
                group_name = $"group{i}",
                created_by = 1,
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestGetGroup()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            if((await response.Content.ReadAsStringAsync()).Contains($"group{i}"))
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestPatchGroup()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}")
            };
            var sendObject = new
            {
                max_user_movies = 4,
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestGetGroup2()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("\"max_user_movies\":4"))
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestGetGroupUsers()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}/users")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            if ((await response.Content.ReadAsStringAsync()).Contains($"\"user_id\":1") && (await response.Content.ReadAsStringAsync()).Contains($"\"is_admin\":true") && (await response.Content.ReadAsStringAsync()).Contains($"\"display_name\":\"afrank1\""))
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestPatchAlias()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/1/{i}/alias")
            };
            var sendObject = new
            {
                alias = $"alias{i}"
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestGetGroupUsers2()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}/users")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            if ((await response.Content.ReadAsStringAsync()).Contains($"\"user_id\":1") && (await response.Content.ReadAsStringAsync()).Contains($"\"display_name\":\"alias{i}\""))
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestPatchAdmin()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/1/{i}/admin")
            };
            var sendObject = new
            {
                is_admin = Convert.ToBoolean((i+1)%2)
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestGetGroupUsers3()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}/users")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            if ((await response.Content.ReadAsStringAsync()).Contains($"\"user_id\":1") && (await response.Content.ReadAsStringAsync()).Contains($"\"is_admin\":{Convert.ToBoolean((i + 1) % 2).ToString().ToLower()}"))
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestRemoveUserGroup()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/1/{i}")
            };
            var sendObject = new
            {
                
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestGetGroupUsers4()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}/users")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            if ((await response.Content.ReadAsStringAsync()).Contains($"\"user_id\":1"))
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestJoinGroup()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/1/join/{i}")
            };
            var sendObject = new
            {
                alias = $"alias2{i}",
                is_admin = Convert.ToBoolean(i % 2),
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestGetGroupUsers5()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}/users")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            if ((await response.Content.ReadAsStringAsync()).Contains($"\"user_id\":1") && (await response.Content.ReadAsStringAsync()).Contains($"\"display_name\":\"alias2{i}\"") && (await response.Content.ReadAsStringAsync()).Contains($"\"is_admin\":{Convert.ToBoolean(i % 2).ToString().ToLower()}"))
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestDeleteGroup()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestGetGroupUsers6()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}/users")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return true;
            }
            if ((await response.Content.ReadAsStringAsync()) == "[]")
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> TestGetGroup3()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{i}")
            };
            var sendObject = new
            {

            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
