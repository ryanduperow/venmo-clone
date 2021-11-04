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

        public TransfersController(ITransferDao _transferDao, IUserDao userDao)
        {
            transferDao = _transferDao;
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
        public Transfer CreateTransfer(Transfer transfer)
        {
            string userName = User.Identity.Name;
            int userId = userDao.GetUser(userName).UserId;
            Transfer createdTransfrer = transferDao.TransferMoney(userId, );

        }
    }
}
