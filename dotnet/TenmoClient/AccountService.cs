using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;
using RestSharp.Authenticators;

namespace TenmoClient
{
    public class AccountService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public decimal GetBalance()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            decimal output = 0;

            RestRequest request = new RestRequest(API_BASE_URL + "account/balance");
            IRestResponse<decimal> response = client.Get<decimal>(request);
            output = response.Data;

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return 0;
            }
            else if (!response.IsSuccessful)
            {               
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                return 0;
            }
            else
            {
                return output;
            }            
        }

        public IList<ListUser> GetUsers()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            RestRequest request = new RestRequest(API_BASE_URL + "users");
            IRestResponse<IList<ListUser>> response = client.Get<IList<ListUser>>(request);
            return response.Data;
        }
    }
}
