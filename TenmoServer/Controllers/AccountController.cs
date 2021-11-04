using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using TenmoServer.DAO;
using Microsoft.AspNetCore.Authorization;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountSqlDAO accountDao;

        public AccountController(IAccountSqlDAO _accountDao)
        {
            this.accountDao = _accountDao;
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult AccountBalance(int userId)
        {
            Account account = accountDao.GetAccountBalance(userId); 

            return Ok(account);
        }
    }
}
