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

        public TransferController(ITransferSqlDAO _transferDAO)
        {
            this.transferDAO = _transferDAO;
        }

        [HttpPost]
        [Authorize]
        public IActionResult MakeTransfer(Transfers newTransfer)
        {
            Transfers result = transferDAO.MakeTransfer(newTransfer);
            return Ok(result);
        }
    }
}
