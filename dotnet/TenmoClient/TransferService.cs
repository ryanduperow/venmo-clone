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

        public Transfer CreateNewTransfer(int fromAccountNumber, int toAccountNumber, int transferType, decimal amount)
        {
            
            // Need to know:
            // The sending user's account number (we have)
            // The recipient user's account number (the user provides this)
            // the transfer type (2/'send' by default)
            // the amount (the user provides this)

            Transfer transfer = new Transfer();
            transfer.AccountFrom = fromAccountNumber;
            transfer.AccountTo = toAccountNumber;
            transfer.TransferTypeID = transferType;
            transfer.Amount = amount;

            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            RestRequest request = new RestRequest(API_BASE_URL + "transfers/new");
            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            return response.Data;
        }
    }
}
