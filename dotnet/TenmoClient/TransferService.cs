using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient
{
    class TransferService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public IList<Transfer> GetTransferList()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            RestRequest request = new RestRequest(API_BASE_URL + "transfers");
            IRestResponse<IList<Transfer>> response = client.Get<IList<Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                return null;
            }
            else
            {
                return response.Data;
            }
        }

        public string GetTransferById(int transferId)
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            RestRequest request = new RestRequest(API_BASE_URL + $"transfers/{transferId}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                return "An error response was received from the server. The status code is " + (int)response.StatusCode;
            }
            else
            {
                return response.Data.FormattedTransfer;
            }
        }

        public string CreateNewTransfer(Transfer transfer)
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            RestRequest request = new RestRequest(API_BASE_URL + "transfers/new");
            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            // RestSharp seems to return an 'error' status when recieving 400-series errors;
            // we altered the standard format here to check only whether the request was successful, not whether it was completed
            // (which could potentially be an issue if incomplete requests are not implicitly unsuccesful.)
            //if (response.ResponseStatus != ResponseStatus.Completed)
            //{
            //    Console.WriteLine("An error occurred communicating with the server.");
            //    return null;
            //}
            if (!response.IsSuccessful)
            {
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                return null;
            }
            else
            {
                return response.Data.FormattedTransfer;
            }
        }
    }
}