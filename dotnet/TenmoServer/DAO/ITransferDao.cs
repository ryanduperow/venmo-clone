using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        public Transfer TransferMoney(Transfer transfer);
        public IList<Transfer> GetAllUserTransfers(int userId);
        public Transfer GetTransferById(int transferId);
    }
}
