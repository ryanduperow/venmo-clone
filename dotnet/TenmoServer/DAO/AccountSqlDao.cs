using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountSqlDao
    {

        private readonly string connectionString;

        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public void DepositToAccount(int userId, decimal amountToDeposit)
        {
            decimal balance = GetAccountBalance(userId);
            balance += amountToDeposit;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE accounts SET balance = @balance WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@balance", balance);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public decimal GetAccountBalance(int userId)
        {
            decimal balance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        balance = Convert.ToDecimal(reader["balance"]);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return balance;
        }
         

        public bool WithdrawFromAccount(int userId, decimal amountToWithdraw)
        {
            decimal balance = GetAccountBalance(userId);
            if (amountToWithdraw > balance)
            {
                return false;
            }

            balance -= amountToWithdraw;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE accounts SET balance = @balance WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@balance", balance);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return true;
        }
    }
}
