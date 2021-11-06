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
        }

        [HttpPost("new")]
        public IActionResult CreateTransfer(Transfer transfer)
        {
            User user = userDao.GetUser(User.Identity.Name);

            // Make sure the transfer type ID and status are 2
            if (transfer.TransferTypeID != 2 || transfer.TransferStatusID != 2)
            {
                return BadRequest("You may only create a transfer with 'send' type and 'approved' status.");
            }

            // Make sure the transfer amount is greater than zero
            if (transfer.Amount <= 0)
            {
                return BadRequest("Transfer amount must be greater than 0.00.");
            }

            // Check to make sure the 'from' account is the same as the logged in user's
            if (transfer.AccountFrom != user.AccountId)
            {
                return BadRequest("You may only create a transfer from your own account.");
            }

            // Check to make sure the 'to' account exists
            Account toAccount = accountDao.GetAccountById(transfer.AccountTo);
            if (toAccount == null)
            {
                return NotFound("You cannot transfer money to a nonexistent account.");
            }

            // Make sure the account has sufficient funds for the transfer
            Account userAccount = accountDao.GetAccountById(user.AccountId);
            if (userAccount.Balance < transfer.Amount)
            {
                return BadRequest("Your account contains insufficient funds for transfer.");
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
