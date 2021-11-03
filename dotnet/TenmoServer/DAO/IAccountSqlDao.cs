using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public interface IAccountSqlDao
    {
        public decimal GetAccountBalance(int userId);
        public bool WithdrawFromAccount(int userId, decimal amountToWithdraw);
        public void DepositToAccount(int userId, decimal amountToDeposit);
    }
}
