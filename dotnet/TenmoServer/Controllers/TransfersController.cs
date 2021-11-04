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

        public TransfersController(ITransferDao _transferDao, IUserDao _userDao)
        {
            transferDao = _transferDao;
            userDao = _userDao;
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
            Transfer createdTransfrer = transferDao.TransferMoney(transfer);
            // TODO: Add validation to make sure the from account is the same as the logged in user
            return Created($"{createdTransfrer.TransferID}", createdTransfrer);

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
