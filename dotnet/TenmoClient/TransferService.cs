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
            return response.Data;
        }

        public Transfer CreateNewTransfer(Transfer transfer)
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            RestRequest request = new RestRequest(API_BASE_URL + "transfers/new");
            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            return response.Data;
        }
    }
}
