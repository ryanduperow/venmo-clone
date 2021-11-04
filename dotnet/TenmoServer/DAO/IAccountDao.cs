using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        public decimal GetAccountBalance(int userId);
        public bool WithdrawFromAccount(int accountId, decimal amountToWithdraw);
        public void DepositToAccount(int accountId, decimal amountToDeposit);
        public Account GetAccountById(int accountId);
    }
}
