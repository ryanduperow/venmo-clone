using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Transfer> GetAllUserTransfers(int userId)
        {
            List<Transfer> listOfTransfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers WHERE account_from = @account_id OR account_to = @account_id", conn);
                    cmd.Parameters.AddWithValue("@account_id", );
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User u = GetUserFromReader(reader);
                        returnUsers.Add(u);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnUsers;
        }

        public Transfer GetTransferById(int transferId)
        {
            throw new NotImplementedException();
        }

        public void TransferMoney(int fromUserId, int toUserId, decimal transferAmount)
        {
            throw new NotImplementedException();
        }
    }
}
