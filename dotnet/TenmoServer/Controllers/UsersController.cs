using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly IUserDao userDao;

        public UsersController(IUserDao _userDao)
        {
            userDao = _userDao;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            IList<User> listOfUsers = userDao.GetUsersPublicFacing();
            if (listOfUsers != null)
            {
                return Ok(listOfUsers);
            }
            else
            {
                return NotFound();
            }
            //todo: do we actually need this error checking?
        }
    }
}
