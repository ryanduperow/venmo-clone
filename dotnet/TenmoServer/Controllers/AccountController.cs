using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class AccountController : ControllerBase
    {
        private readonly IAccountDao accountDao;
        private readonly IUserDao userDao;

        public AccountController(IAccountDao _accountDao, IUserDao _userDao)
        {
            accountDao = _accountDao;
            userDao = _userDao;
        }

        [HttpGet("balance")]
        public IActionResult GetBalance()
        {
            string userName = User.Identity.Name;
            int userId = userDao.GetUser(userName).UserId;

            decimal accountBalance = accountDao.GetAccountBalance(userId);
            return Ok(accountBalance);
            //TODO: figure out what this actually does

            //int accountID = userSqlDao.GetUserAccountID(userId);
            //TODO: There is a way to get the userID 
            //string userName = User.Identity.Name;
            //decimal accountBalance = accountDao.GetAccountBalance(userId);
            // TODO: Do we need to send a bad request message back if the user doesn't exist or something?
        }
    }
}
