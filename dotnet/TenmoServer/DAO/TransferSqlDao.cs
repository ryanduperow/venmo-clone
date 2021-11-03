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

            //TODO: Call this only once or not at all
            UserSqlDao userSqlDao = new UserSqlDao(connectionString);
            int accountID = userSqlDao.GetUserAccountID(userId);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers WHERE account_from = @account_id OR account_to = @account_id", conn);
                    cmd.Parameters.AddWithValue("@account_id", accountID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer currentTransfer = GetTransferFromReader(reader);
                        listOfTransfers.Add(currentTransfer);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return listOfTransfers;
        }

        public Transfer GetTransferById(int transferId)
        {
            Transfer transfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers WHERE transfer_id = transfer_id;", conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transferId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = GetTransferFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transfer;
        }

        public void TransferMoney(int fromUserId, int toUserId, int transferTypeId, decimal transferAmount)
        {
            UserSqlDao userSqlDao = new UserSqlDao(connectionString);
            int fromAccountID = userSqlDao.GetUserAccountID(fromUserId);
            int toAccountID = userSqlDao.GetUserAccountID(toUserId);
         
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //TODO: if they ever change what transfer status 1 means, this will break
                    SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES(@transfer_type_id, 1, @account_from, @account_to, @amount); ", conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transferTypeId);
                    cmd.Parameters.AddWithValue("@account_from", fromAccountID);
                    cmd.Parameters.AddWithValue("@account_to", toAccountID);
                    cmd.Parameters.AddWithValue("@amount", transferAmount);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            return new Transfer()
            {
                TransferID = Convert.ToInt32(reader["transfer_id"]),
                TransferTypeID = Convert.ToInt32(reader["transfer_type_id"]),
                TransferStatusID = Convert.ToInt32(reader["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                AccountTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"]),
            };
        }
    }
}
