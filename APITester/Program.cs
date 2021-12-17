using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace APITester
{
    class Program
    {
        private static string baseAddress = "https://localhost:44331/";

        static async Task Main(string[] args)
        {
         await Tests();
            //await OneTimeTest();
        }

        private static string RandomString(int length = 10)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }

        private static async Task OneTimeTest()
        {
            //README FOR BACKEND

            var client = new HttpClient()
            {
                //BaseAddress = new Uri("http://localhost:26585/login")
                //BaseAddress = new Uri("https://localhost:44346/property/manager/2")
                //BaseAddress = new Uri("https://spikeexercise.azurewebsites.net/property")
                BaseAddress = new Uri("https://localhost:44331/user/1/join")
                //BaseAddress = new Uri("https://movienightapi.azurewebsites.net/user/1/join")
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
                /*username = "afrank",
                password = "password",
                email = "afrank@testemailtesttest.com",*/
                alias = "dsfgfds",
                is_admin = false,
                group_code = 5
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
            string username_1 = RandomString();
            string password_1 = RandomString();
            string email_1 = RandomString() + "@gmail.com";
            string token_1 = "";
            int user_id_1 = -1;


            string username_2 = RandomString();
            string password_2 = RandomString();
            string email_2 = RandomString() + "@gmail.com";
            string token_2 = "";
            int user_id_2 = -1;

            string username_3 = RandomString();
            string password_3 = RandomString();
            string email_3 = RandomString() + "@gmail.com";
            string token_3 = "";
            int user_id_3 = -1;

            int group_id_1 = -1;
            string group_code_1 = "";

            int group_id_2 = -1;
            string group_code_2 = "";

            int event_id_1 = -1;

            Console.WriteLine($"TestSignUpNew: {await TestSignUpNew(username_1, password_1, email_1)}");
            Console.WriteLine($"TestSignUpSecondUser: {await TestSignUpSecondUser(username_2, password_2, email_2)}");
            Console.WriteLine($"TestSignUpExistingUser: {await TestSignUpExistingUser(username_1)}");
            Console.WriteLine($"TestLoginValidUser1: {await TestLoginValidUser1(username_1, password_1, x => token_1 = x, y => user_id_1 = y)}");
            Console.WriteLine($"TestLoginIncorrectUsername: {await TestLoginIncorrectUsername()}");
            Console.WriteLine($"TestLoginIncorrectPassword: {await TestLoginIncorrectPassword(username_1)}");
            Console.WriteLine($"TestLoginValidUser2: {await TestLoginValidUser2(username_2, password_2, x => token_2 = x, y => user_id_2 = y)}");
            Console.WriteLine($"TestAddFirstGroup: {await TestAddFirstGroup(user_id_1, token_1, x => group_id_1 = x, y => group_code_1 = y)}");
            Console.WriteLine($"TestAddSecondGroup: {await TestAddSecondGroup(user_id_1, token_1, x => group_id_2 = x, y => group_code_2 = y)}");
            Console.WriteLine($"TestGetGroup: {await TestGetGroup(token_1, group_id_1, user_id_1, group_code_1)}");
            Console.WriteLine($"TestPatchMaxUserMovies: {await TestPatchMaxUserMovies(token_1, group_id_1)}");
            Console.WriteLine($"TestPatchGroupName: {await TestPatchGroupName(token_1, group_id_1)}");
            Console.WriteLine($"TestJoinGroup: {await TestJoinGroup(token_2, user_id_2, group_code_1, group_id_1)}");
            Console.WriteLine($"TestJoinGroupAgain: {await TestJoinGroupAgain(token_2, user_id_2, group_code_1)}");
            Console.WriteLine($"TestGetGroupUsers: {await TestGetGroupUsers(token_1, group_id_1, user_id_1, user_id_2)}");
            Console.WriteLine($"TestPatchGroupCode: {await TestPatchGroupCode(token_1, group_id_1, group_code_1, x => group_code_1 = x)}");
            Console.WriteLine($"TestPatchAlias: {await TestPatchAlias(token_1, user_id_1, group_id_1)}");
            Console.WriteLine($"TestPatchAdmin: {await TestPatchAdmin(token_2, user_id_2, group_id_1)}");
            Console.WriteLine($"TestPostMovie: {await TestPostMovie(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestPostMovieAgain: {await TestPostMovieAgain(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestPostSecondMovie: {await TestPostSecondMovie(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestPostMovieTooMany: {await TestPostMovieTooMany(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestPostMovieNewUser: {await TestPostMovieNewUser(token_2, group_id_1, user_id_2)}");
            Console.WriteLine($"TestPatchMovieRating: {await TestPatchMovieRating(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestPatchMovieRatingInvalid1: {await TestPatchMovieRatingInvalid1(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestPatchMovieRatingInvalid2: {await TestPatchMovieRatingInvalid2(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestPatchMovieRatingInvalid3: {await TestPatchMovieRatingInvalid3(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestGetMovieRatings: {await TestGetMovieRatings(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestCreateEvent: {await TestCreateEvent(token_1, group_id_1, user_id_1, x => event_id_1 = x)}");
            Console.WriteLine($"TestEventRSVP: {await TestEventRSVP(token_1, event_id_1, user_id_1)}");
            Console.WriteLine($"TestEventRSVPSecondUser: {await TestEventRSVPSecondUser(token_2, event_id_1, user_id_1, user_id_2)}");
            Console.WriteLine($"TestEventPatchRSVP1: {await TestEventPatchRSVP1(token_1, event_id_1, user_id_1)}");
            Console.WriteLine($"TestEventRSVPGet: {await TestEventRSVPGet(token_1, event_id_1, user_id_1, user_id_2)}");
            Console.WriteLine($"TestEventPatchRSVP2: {await TestEventPatchRSVP2(token_1, event_id_1, user_id_1)}");
            Console.WriteLine($"TestEventPatchVotingMode: {await TestEventPatchVotingMode(token_1, event_id_1)}");
            Console.WriteLine($"TestEventPatchMovie: {await TestEventPatchMovie(token_1, event_id_1)}");
            Console.WriteLine($"TestGetEvent: {await TestGetEvent(token_1, event_id_1)}");
            Console.WriteLine($"TestEventAddMovies: {await TestEventAddMovies(token_1, event_id_1)}");
            Console.WriteLine($"TestEventChangeRatings: {await TestEventChangeRatings(token_1, event_id_1, user_id_1)}");
            Console.WriteLine($"TestEventGetUserRatings: {await TestEventGetUserRatings(token_1, event_id_1, user_id_1)}");
            Console.WriteLine($"TestEventGetAvgRatings: {await TestEventGetAvgRatings(token_1, event_id_1)}");

            Console.WriteLine($"TestSignUpSecondUser: {await TestSignUpThirdUser(username_3, password_3, email_3)}");
            Console.WriteLine($"TestLoginValidUser3: {await TestLoginValidUser3(username_3, password_3, x => token_3 = x, y => user_id_3 = y)}");
            Console.WriteLine($"TestJoinGroupThirdUser: {await TestJoinGroupThirdUser(token_3, user_id_3, group_code_1, group_id_1)}");
            Console.WriteLine($"TestEventRSVPThirdUser: {await TestEventRSVPThirdUser(token_3, event_id_1, user_id_3)}");
            Console.WriteLine($"TestEventGetUserRatings3: {await TestEventGetUserRatings3(token_3, event_id_1, user_id_3)}");
            Console.WriteLine($"TestEventPatchRSVP3: {await TestEventPatchRSVP3(token_3, event_id_1, user_id_3)}");
            Console.WriteLine($"TestEventGetUserRatingsInvalid: {await TestEventGetUserRatingsInvalid(token_3, event_id_1, user_id_3)}");

            Console.WriteLine($"TestDeleteEvent: {await TestDeleteEvent(token_1, event_id_1)}");
            Console.WriteLine($"TestDeleteMovie1: {await TestDeleteMovie1(token_1, group_id_1)}");
            Console.WriteLine($"TestDeleteMovie2: {await TestDeleteMovie2(token_1, group_id_1)}");
            Console.WriteLine($"TestDeleteMovie3: {await TestDeleteMovie3(token_1, group_id_1)}");
            Console.WriteLine($"TestDeleteMovieInvalid: {await TestDeleteMovieInvalid(token_1, group_id_1)}");
            Console.WriteLine($"TestGetMovieRatingsNoMovies: {await TestGetMovieRatingsNoMovies(token_1, group_id_1, user_id_1)}");
            Console.WriteLine($"TestGetUserGroups: {await TestGetUserGroups(token_1, user_id_1, group_id_1, group_id_2)}");
            Console.WriteLine($"TestDeleteUserGroup1: {await TestDeleteUserGroup1(token_1, user_id_1, group_id_1)}");
            Console.WriteLine($"TestGetUserGroup: {await TestGetUserGroup(token_1, user_id_1, group_id_1, group_id_2)}");
            Console.WriteLine($"TestDeleteUserGroup2: {await TestDeleteUserGroup2(token_1, user_id_1, group_id_2)}");
            Console.WriteLine($"TestGetUserGroupsNone: {await TestGetUserGroupsNone(token_1, user_id_1)}");
            Console.WriteLine($"TestGetGroupUsersEmpty: {await TestGetGroupUsersEmpty(token_1, group_id_2)}");
            Console.WriteLine($"TestDeleteGroup: {await TestDeleteGroup(token_1, group_id_2)}");
            Console.WriteLine($"TestDeleteGroupInvalid: {await TestDeleteGroupInvalid(token_1)}");
            Console.WriteLine($"TestGetUserGroupsWrongToken: {await TestGetUserGroupsWrongToken(token_1, user_id_2)}");
        }

        //Console.WriteLine((int) response.StatusCode);
        //Console.WriteLine(await response.Content.ReadAsStringAsync());

        // A normal, valid signup
        private static async Task<bool> TestSignUpNew(string username_1, string password_1, string email_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "signup")
            };
            var sendObject = new
            {
                username = username_1,
                password = password_1,
                email = email_1
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

        // Sign up another valid user
        private static async Task<bool> TestSignUpSecondUser(string username_2, string password_2, string email_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "signup")
            };
            var sendObject = new
            {
                username = username_2,
                password = password_2,
                email = email_2
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

        // Try to signup a user with a username that already exists
        private static async Task<bool> TestSignUpExistingUser(string username_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "signup")
            };
            var sendObject = new
            {
                username = username_1,
                password = RandomString(),
                email = RandomString() + "@gmail.com"
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

        // Try to login a user that exists
        private static async Task<bool> TestLoginValidUser1(string username_1, string password_1, Action<string> token_setter, Action<int> user_id_setter)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "login")
            };
            var sendObject = new
            {
                username = username_1,
                password = password_1,
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);

            // Get the token and user_id for this user
            string response_string = await response.Content.ReadAsStringAsync();
            string[] responses = response_string.ToString().Split(',');
            token_setter(responses[responses.Length - 1].Substring(9, responses[responses.Length - 1].Length - 11));
            user_id_setter(int.Parse(responses[0].Substring(11, responses[0].Length - 11)));
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        // Try to login with a username that does not exist
        private static async Task<bool> TestLoginIncorrectUsername()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "login")
            };
            var sendObject = new
            {
                username = RandomString(),
                password = RandomString(),
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

        // Try to login a valid username with an incorrect password
        private static async Task<bool> TestLoginIncorrectPassword(string username_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "login")
            };
            var sendObject = new
            {
                username = username_1,
                password = RandomString(),
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

        // Try to login another user that exists
        private static async Task<bool> TestLoginValidUser2(string username_2, string password_2, Action<string> token_setter, Action<int> user_id_setter)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "login")
            };
            var sendObject = new
            {
                username = username_2,
                password = password_2,
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);

            // Get the token and user_id for this user
            string response_string = await response.Content.ReadAsStringAsync();
            string[] responses = response_string.ToString().Split(',');
            token_setter(responses[responses.Length - 1].Substring(9, responses[responses.Length - 1].Length - 11));
            user_id_setter(int.Parse(responses[0].Substring(11, responses[0].Length - 11)));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        // Creates a group
        private static async Task<bool> TestAddFirstGroup(int user_id_1, string token_1, Action<int> group_id_setter, Action<string> group_code_setter)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "group")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                group_name = RandomString(),
                created_by = user_id_1,
                alias = RandomString()
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

            // Get the group code and id for this group
            string response_string = await response.Content.ReadAsStringAsync();
            string[] responses = response_string.ToString().Split(',');
            group_id_setter(int.Parse(responses[2].Substring(11, responses[2].Length - 11)));
            group_code_setter(responses[3].Substring(14, responses[3].Length - 15));

            return true;
        }

        // Creates a second group
        private static async Task<bool> TestAddSecondGroup(int user_id_1, string token_1, Action<int> group_id_setter, Action<string> group_code_setter)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "group")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                group_name = RandomString(),
                created_by = user_id_1,
                alias = RandomString()
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

            // Get the group code and id for this group
            string response_string = await response.Content.ReadAsStringAsync();
            string[] responses = response_string.ToString().Split(',');
            group_id_setter(int.Parse(responses[2].Substring(11, responses[2].Length - 11)));
            group_code_setter(responses[3].Substring(14, responses[3].Length - 15));

            return true;
        }

        // Attempts to get a valid group
        private static async Task<bool> TestGetGroup(string token_1, int group_id_1, int user_id_1, string group_code_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains($"\"created_by\":{user_id_1}") &
                response_string.Contains("\"max_user_movies\":5") &
                response_string.Contains($"\"group_code\":\"{group_code_1}\""))
            {
                return true;
            }
            return false;
        }

        // Change the max_user_movies of a group
        private static async Task<bool> TestPatchMaxUserMovies(string token_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/max_user_movies")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                max_user_movies = 2,
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
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains("\"max_user_movies\":2"))
            {
                return true;
            }

            return false;
        }

        // Changes the name of a group
        private static async Task<bool> TestPatchGroupName(string token_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/group_name")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            string new_name = RandomString();
            var sendObject = new
            {
                group_name = new_name
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
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_name\":\"{new_name}\""))
            {
                return true;
            }
            return false;
        }

        // Tests a second user joining an existing group
        private static async Task<bool> TestJoinGroup(string token_2, int user_id_2, string group_code_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_2}/join")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_2);
            var sendObject = new
            {
                group_code = group_code_1,
                alias = RandomString(),
                is_admin = false
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
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains("\"is_admin\":false"))
            {
                return true;
            }
            return false;
        }

        // Tests a user joining a group they already are a member of
        private static async Task<bool> TestJoinGroupAgain(string token_2, int user_id_2, string group_code_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_2}/join")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_2);
            var sendObject = new
            {
                group_code = group_code_1,
                alias = RandomString(),
                is_admin = false
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

        // Attempts to get a list of all users in the given group
        private static async Task<bool> TestGetGroupUsers(string token_1, int group_id_1, int user_id_1, int user_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/users")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_2}")
                )
            {
                return true;
            }
            return false;
        }

        // Changes the code of a group
        private static async Task<bool> TestPatchGroupCode(string token_1, int group_id_1, string group_code_1, Action<string> group_code_setter)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/code")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                
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

            // Get the group code for this group
            string response_string = await response.Content.ReadAsStringAsync();
            string[] responses = response_string.ToString().Split(',');
            group_code_setter(responses[4].Substring(14, responses[3].Length - 13));

            // Make sure that the group code has changed
            if (response_string.Contains($"\"group_code\":\"{group_code_1}\""))
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> TestPatchAlias(string token_1, int user_id_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/{group_id_1}/alias")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            string new_alias = RandomString();
            var sendObject = new
            {
                alias = new_alias
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
            // Make sure that the alias has changed
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"display_name\":\"{new_alias}\""))
            {
                return true;
            }
            return false;
        }

        // Test changing a user's admin status in a group
        private static async Task<bool> TestPatchAdmin(string token_2, int user_id_2, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_2}/{group_id_1}/admin")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_2);
            var sendObject = new
            {
                is_admin = true
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
            // Make sure that the admin status has changed
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains("\"is_admin\":true"))
            {
                return true;
            }
            return false;
        }

        // Test adding a movie to a group
        private static async Task<bool> TestPostMovie(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 1,
                added_by = user_id_1
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
            // Make sure that the return values are correct
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains("\"tmdb_movie_id\":1") &
                response_string.Contains($"\"added_by\":{user_id_1}")
                )
            {
                return true;
            }
            return false;
        }

        // Test adding the same movie to a group
        private static async Task<bool> TestPostMovieAgain(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 1,
                added_by = user_id_1
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

        // Test adding another movie to the group
        private static async Task<bool> TestPostSecondMovie(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 2,
                added_by = user_id_1
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
            // Make sure that the return values are correct
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains("\"tmdb_movie_id\":2") &
                response_string.Contains($"\"added_by\":{user_id_1}")
                )
            {
                return true;
            }
            return false;
        }

        // Test adding a movie when max_user movies has been reached
        private static async Task<bool> TestPostMovieTooMany(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 3,
                added_by = user_id_1
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


        // Test adding a movie from another user
        private static async Task<bool> TestPostMovieNewUser(string token_2, int group_id_1, int user_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_2);
            var sendObject = new
            {
                tmdb_movie_id = 3,
                added_by = user_id_2
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
            // Make sure that the return values are correct
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains("\"tmdb_movie_id\":3") &
                response_string.Contains($"\"added_by\":{user_id_2}")
                )
            {
                return true;
            }
            return false;
        }

        // Test changing the rating of a movie
        private static async Task<bool> TestPatchMovieRating(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/{group_id_1}/rating")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 3,
                user_rating = 10
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
            // Make sure that the return values are correct
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_1}") &
                response_string.Contains("\"tmdb_movie_id\":3") &
                response_string.Contains("\"user_rating\":10")
                )
            {
                return true;
            }
            return false;
        }

        // Test changing the rating of a movie that doesn't exist
        private static async Task<bool> TestPatchMovieRatingInvalid1(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/{group_id_1}/rating")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 5,
                user_rating = 10
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        // Test leaving a rating above 10
        private static async Task<bool> TestPatchMovieRatingInvalid2(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/{group_id_1}/rating")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 3,
                user_rating = 11
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        // Test leaving a rating below 0
        private static async Task<bool> TestPatchMovieRatingInvalid3(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/{group_id_1}/rating")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 3,
                user_rating = -1
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        // Test getting the reviews for a group
        private static async Task<bool> TestGetMovieRatings(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movies/{user_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            // Make sure that the return values are correct
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains("\"tmdb_movie_id\":1") &
                response_string.Contains("\"tmdb_movie_id\":2") &
                response_string.Contains("\"tmdb_movie_id\":3") &
                response_string.Contains("\"user_rating\":5") &
                response_string.Contains("\"user_rating\":10") &
                response_string.Contains("\"avg_user_rating\":7.5")
                )
            {
                return true;
            }
            return false;
        }

        // Test creating an event
        private static async Task<bool> TestCreateEvent(string token_1, int group_id_1, int user_id_1, Action<int> event_id_setter)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            String loc = RandomString();
            var sendObject = new
            {
                group_id = group_id_1,
                start_time = DateTime.Now,
                location = loc,
                genres = new List<int> { 5, 3, 2 },
                organized_by = 1,
                voting_mode = 1,
                services = new List<int> { 8, 4, 7 }
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
            // Make sure that the return values are correct
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains($"\"location\":\"{loc}\"") &
                response_string.Contains("\"genres\":[5,3,2]") &
                response_string.Contains("\"organized_by\":1") &
                response_string.Contains("\"voting_mode\":1") &
                response_string.Contains("\"services\":[8,4,7]")
                )
            {
                event_id_setter(int.Parse(Regex.Match(response_string, @"\d+").Value));
                return true;
            }
            return false;
        }

        // Test RSVP'ing a user to an event
        private static async Task<bool> TestEventRSVP(string token_1, int event_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/rsvp")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                event_id = event_id_1,
                user_id = user_id_1,
                is_coming = true
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_1}") &
                response_string.Contains("\"is_coming\":true")
                )
            {
                return true;
            }
            return false;
        }

        // Test RSVP'ing a second user to an event
        private static async Task<bool> TestEventRSVPSecondUser(string token_2, int event_id_1, int user_id_1, int user_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/rsvp")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_2);
            var sendObject = new
            {
                event_id = event_id_1,
                user_id = user_id_2,
                is_coming = true
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_1}") &
                response_string.Contains("\"is_coming\":true") &
                response_string.Contains($"\"user_id\":{user_id_2}") &
                !response_string.Contains("\"is_coming\":false")
                )
            {
                return true;
            }
            return false;
        }

        // Test un-RSVP'ing a user to an event
        private static async Task<bool> TestEventPatchRSVP1(string token_1, int event_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/rsvp/{user_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                is_coming = false
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_1}") &
                response_string.Contains("\"is_coming\":true") &
                response_string.Contains("\"is_coming\":false")
                )
            {
                return true;
            }
            return false;
        }

        // Test get the RSVP status of an event
        private static async Task<bool> TestEventRSVPGet(string token_1, int event_id_1, int user_id_1, int user_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/rsvp")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new {};
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_2}") &
                response_string.Contains("\"is_coming\":true") &
                response_string.Contains("\"is_coming\":false")
                )
            {
                return true;
            }
            return false;
        }

        // Test re-RSVP'ing a user to an event
        private static async Task<bool> TestEventPatchRSVP2(string token_1, int event_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/rsvp/{user_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                is_coming = true
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_1}") &
                response_string.Contains("\"is_coming\":true") &
                !response_string.Contains("\"is_coming\":false")
                )
            {
                return true;
            }
            return false;
        }

        // Test changing the voting mode of an event
        private static async Task<bool> TestEventPatchVotingMode(string token_1, int event_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                voting_mode = 2
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains("\"voting_mode\":2")
                )
            {
                return true;
            }
            return false;
        }

        // Test changing the voting mode of an event
        private static async Task<bool> TestEventPatchMovie(string token_1, int event_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/movie")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new
            {
                tmdb_movie_id = 1324
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains("\"tmdb_movie_id\":1324")
                )
            {
                return true;
            }
            return false;
        }

        // Test changing the voting mode of an event
        private static async Task<bool> TestGetEvent(string token_1, int event_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            var sendObject = new {};
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains("\"voting_mode\":2") &
                response_string.Contains("\"services\":[4,7,8]") &
                response_string.Contains("\"genres\":[2,3,5]")
                )
            {
                return true;
            }
            return false;
        }

        // Test adding movies to an event
        private static async Task<bool> TestEventAddMovies(string token_1, int event_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/movies")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            int[] mov_ids = { 1, 2, 3 };
            var sendObject = new
            {
                tmdb_movie_ids = mov_ids
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains("\"tmdb_movie_id\":1") &
                response_string.Contains("\"tmdb_movie_id\":2") &
                response_string.Contains("\"tmdb_movie_id\":3")
                )
            {
                return true;
            }
            return false;
        }

        // Test changing a user's ratings
        private static async Task<bool> TestEventChangeRatings(string token_1, int event_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/rating")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
            
            var sendObject = new
            {
                event_id = event_id_1,
                user_id = user_id_1,
                tmdb_movie_id = 1,
                rating = 3
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains("\"rating\":2") &
                response_string.Contains("\"rating\":3") &
                response_string.Contains($"\"user_id\":{user_id_1}")
                )
            {
                return true;
            }
            return false;
        }

        // Test getting a user's ratings
        private static async Task<bool> TestEventGetUserRatings(string token_1, int event_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/movies/{user_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);

            var sendObject = new {};
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains("\"rating\":2") &
                response_string.Contains("\"rating\":3")
                )
            {
                return true;
            }
            return false;
        }

        // Test getting an event's average movie ratings
        private static async Task<bool> TestEventGetAvgRatings(string token_1, int event_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/rating")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);

            var sendObject = new { };
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains("\"avg_rating\":2.5") &
                response_string.Contains("\"avg_rating\":2") &
                response_string.Contains("\"tmdb_movie_id\":1") &
                response_string.Contains("\"tmdb_movie_id\":2") &
                response_string.Contains("\"tmdb_movie_id\":3")
                )
            {
                return true;
            }
            return false;
        }

        // Create a third user
        private static async Task<bool> TestSignUpThirdUser(string username_3, string password_3, string email_3)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "signup")
            };
            var sendObject = new
            {
                username = username_3,
                password = password_3,
                email = email_3
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

        // Try to login the third user
        private static async Task<bool> TestLoginValidUser3(string username_3, string password_3, Action<string> token_setter, Action<int> user_id_setter)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + "login")
            };
            var sendObject = new
            {
                username = username_3,
                password = password_3,
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);

            // Get the token and user_id for this user
            string response_string = await response.Content.ReadAsStringAsync();
            string[] responses = response_string.ToString().Split(',');
            token_setter(responses[responses.Length - 1].Substring(9, responses[responses.Length - 1].Length - 11));
            user_id_setter(int.Parse(responses[0].Substring(11, responses[0].Length - 11)));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        // Enroll the third user in the first group
        private static async Task<bool> TestJoinGroupThirdUser(string token_3, int user_id_3, string group_code_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_3}/join")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_3);
            var sendObject = new
            {
                group_code = group_code_1,
                alias = RandomString(),
                is_admin = false
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
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains("\"is_admin\":false"))
            {
                return true;
            }
            return false;
        }

        // RSVP the third user to the event
        private static async Task<bool> TestEventRSVPThirdUser(string token_3, int event_id_1, int user_id_3)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/rsvp")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_3);
            var sendObject = new
            {
                event_id = event_id_1,
                user_id = user_id_3,
                is_coming = true
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_3}") &
                response_string.Contains("\"is_coming\":true")
                )
            {
                return true;
            }
            return false;
        }

        // Check that the user's ratings were automatically added for the movies
        private static async Task<bool> TestEventGetUserRatings3(string token_3, int event_id_1, int user_id_3)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/movies/{user_id_3}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_3);

            var sendObject = new { };
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains("\"rating\":2") &
                !response_string.Contains("\"rating\":3") &
                !response_string.Contains("\"rating\":1")
                )
            {
                return true;
            }
            return false;
        }

        // Un-RSVP the third user from the event
        private static async Task<bool> TestEventPatchRSVP3(string token_3, int event_id_1, int user_id_3)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/rsvp/{user_id_3}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_3);
            var sendObject = new
            {
                is_coming = false
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}") &
                response_string.Contains($"\"user_id\":{user_id_3}") &
                response_string.Contains("\"is_coming\":true") &
                response_string.Contains("\"is_coming\":false")
                )
            {
                return true;
            }
            return false;
        }

        // Check that their ratings disappear
        private static async Task<bool> TestEventGetUserRatingsInvalid(string token_3, int event_id_1, int user_id_3)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}/movies/{user_id_3}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_3);

            var sendObject = new { };
            var content = new StringContent(JsonConvert.SerializeObject(sendObject).ToString(), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = content
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        // Test getting an event's average movie ratings
        private static async Task<bool> TestDeleteEvent (string token_1, int event_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"event/{event_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);

            var sendObject = new { };
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
            // Make sure that the return values are correct

            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"event_id\":{event_id_1}")
                )
            {
                return true;
            }
            return false;
        }

        // Delete first movie from the group
        private static async Task<bool> TestDeleteMovie1(string token_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie/1")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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

        // Delete second movie from the group
        private static async Task<bool> TestDeleteMovie2(string token_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie/2")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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

        // Delete third movie from the group
        private static async Task<bool> TestDeleteMovie3(string token_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie/3")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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

        // Try to delete a movie not in the group
        private static async Task<bool> TestDeleteMovieInvalid(string token_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movie/10")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        // Get ratings when there are no movies
        private static async Task<bool> TestGetMovieRatingsNoMovies(string token_1, int group_id_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_1}/movies/{user_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            // Make sure that the returned list is empty
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains("[]"))
            {
                return true;
            }
            return false;
        }

        // Get the groups of a user
        private static async Task<bool> TestGetUserGroups(string token_1, int user_id_1, int group_id_1, int group_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/groups")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            // Make sure that the returned list is empty
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains($"\"group_id\":{group_id_2}")
                )
            {
                return true;
            }
            return false;
        }

        // Delete one of the first user's groups
        private static async Task<bool> TestDeleteUserGroup1(string token_1, int user_id_1, int group_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/{group_id_1}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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

        // Get the last group of a user
        private static async Task<bool> TestGetUserGroup(string token_1, int user_id_1, int group_id_1, int group_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/groups")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            // Make sure that the returned list is empty
            string response_string = await response.Content.ReadAsStringAsync();
            if (!response_string.Contains($"\"group_id\":{group_id_1}") &
                response_string.Contains($"\"group_id\":{group_id_2}")
                )
            {
                return true;
            }
            return false;
        }

        // Delete the last of a user's groups
        private static async Task<bool> TestDeleteUserGroup2(string token_1, int user_id_1, int group_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/{group_id_2}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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

        // Get the groups of a user who has no groups
        private static async Task<bool> TestGetUserGroupsNone(string token_1, int user_id_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_1}/groups")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            // Make sure that the returned list is empty
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains("[]"))
            {
                return true;
            }
            return false;
        }

        // Test getting a group's users on an empty group
        private static async Task<bool> TestGetGroupUsersEmpty(string token_1, int group_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_2}/users")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            string response_string = await response.Content.ReadAsStringAsync();
            if (response_string.Contains("[]"))
            {
                return true;
            }
            return false;
        }

        // Test deleting group
        private static async Task<bool> TestDeleteGroup(string token_1, int group_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{group_id_2}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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

        // Test deleting group that doesn't exist
        private static async Task<bool> TestDeleteGroupInvalid(string token_1)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"group/{1000000}")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        // Attempt to access a user's information using the token of another user
        private static async Task<bool> TestGetUserGroupsWrongToken(string token_1, int user_id_2)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress + $"user/{user_id_2}/groups")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_1);
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
            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

    }
}
