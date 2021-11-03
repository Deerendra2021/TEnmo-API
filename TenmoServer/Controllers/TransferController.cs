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
        private readonly IAccountSqlDAO accountDAO;
        private readonly IUserDAO userDAO;

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
