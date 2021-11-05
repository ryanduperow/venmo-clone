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

        public AccountController(IAccountDao _accountDao)
        {
            accountDao = _accountDao;
        }

        [HttpGet("balance")]
        public IActionResult GetBalance()
        {
            int userId = int.Parse(User.FindFirst("sub")?.Value);
            decimal accountBalance = accountDao.GetAccountBalance(userId);
            return Ok(accountBalance);
        }
    }
}
