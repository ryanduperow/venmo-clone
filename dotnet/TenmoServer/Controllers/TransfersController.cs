using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferDao transferDao;
        private readonly IUserDao userDao;
        private readonly IAccountDao accountDao;

        public TransfersController(ITransferDao _transferDao, IUserDao _userDao, IAccountDao _accountDao)
        {
            transferDao = _transferDao;
            userDao = _userDao;
            accountDao = _accountDao;
        }

        [HttpGet]
        public IActionResult GetAllTransfers()
        {
            string userName = User.Identity.Name;
            int userId = userDao.GetUser(userName).UserId;
            return Ok(transferDao.GetAllUserTransfers(userId));
            // TODO: Add error handling in case list is empty
        }

        [HttpPost("new")]
        public IActionResult CreateTransfer(Transfer transfer)
        {
            string userName = User.Identity.Name;
            int userId = userDao.GetUser(userName).UserId;


            // Check to make sure the 'from' account is the same as the logged in user's
            int accountNumber = userDao.GetUserAccountID(userId);
            if (transfer.AccountFrom != accountNumber)
            {
                return Forbid();
            }

            // Check to make sure the 'to' account exists
            Account toAccount = accountDao.GetAccountById(transfer.AccountTo);
            if (toAccount == null)
            {
                return NotFound();
            }

            // Make sure the account has sufficient funds for the transfer
            Account userAccount = accountDao.GetAccountById(accountNumber);
            if (userAccount.Balance < transfer.Amount)
            {
                //TODO: ask Mike or Joe about correct HTTP response code for this
                return Ok();
            }

            Account fromAccount = accountDao.GetAccountById(transfer.AccountFrom);

            //Actually perform the withdrawal and deposit
            accountDao.WithdrawFromAccount(fromAccount.AccountId, transfer.Amount);
            accountDao.DepositToAccount(toAccount.AccountId, transfer.Amount);

            //Create the transfer
            Transfer createdTransfer = transferDao.TransferMoney(transfer);
            return Created($"{createdTransfer.TransferID}", createdTransfer);

        }

        [HttpGet("{transferId}")]
        public IActionResult GetTransferById(int transferId)
        {
            Transfer transfer = transferDao.GetTransferById(transferId);
            if (transfer != null)
            {
                return Ok(transfer); 
            }
            else
            {
                return NotFound();
            }
        }
    }
}
