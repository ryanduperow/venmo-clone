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

        // In GetAllUserTransfers we join the accounts table on the transfers table twice to catch any transfers the userID parameter was associated with-- whether they were the sender or the recipient. (Mike explained this to us)
        public IList<Transfer> GetAllUserTransfers(int userId)
        {
            List<Transfer> listOfTransfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers t JOIN accounts a ON a.account_id = t.account_from JOIN accounts b ON b.account_id = t.account_to WHERE a.user_id = @user_id OR b.user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
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
            Transfer transfer = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers WHERE transfer_id = @transfer_id;", conn);
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

        public Transfer TransferMoney(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) OUTPUT INSERTED.transfer_id VALUES(@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount); ", conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeID);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusID);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    transfer.TransferID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transfer;
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
