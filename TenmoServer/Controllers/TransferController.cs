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
    public class TransferController : ControllerBase
    {
        private readonly ITransferSqlDAO transferDAO;
        private readonly IAccountSqlDAO accountDAO; // Not using these, consider deleting
        private readonly IUserDAO userDAO; // Not using these, consider deleting

        public TransferController(ITransferSqlDAO _transferDAO, IAccountSqlDAO _accountDAO, IUserDAO _userDAO)
        {
            this.transferDAO = _transferDAO;
            this.accountDAO = _accountDAO;
            this.userDAO = _userDAO;
        }

        [HttpPost]
        [Authorize]
        public IActionResult MakeTransfer(Transfers newTransfer)
        {
            Transfers result = transferDAO.MakeTransfer(newTransfer);

            int userId = int.Parse(this.User.FindFirst("sub").Value);

            if (userId == newTransfer.AccountToUserId)
            {
                return BadRequest("You cannot transfer money from other people's account to your own account");
            }

            return Ok(result);
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult ViewTransfers(int userId)
        {
            List<Transfers> result = transferDAO.ViewTransfers(userId);
            return Ok(result);
        }
    }
}
