using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class AccountController : ControllerBase
    {
        private readonly IAccountDao accountDao;

        public AccountController(IAccountDao _accountDao)
        {
            accountDao = _accountDao;
        }

        [HttpGet("balance")]
        public IActionResult GetBalance(ReturnUser userParam)
        {
            int userId = userParam.UserId;
            decimal accountBalance = accountDao.GetAccountBalance(userId);

            // TODO: Do we need to send a bad request message back if the user doesn't exist or something?

            return Ok(accountBalance);
        }
    }
}
