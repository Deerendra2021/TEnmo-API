using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class Transfer
    {
        public int Transfer_Id { get; set; }

        public int Transfers_Type_Id { get; set; }

        public int Transfers_Status_Id { get; set; }

        public int Account_From { get; set; }

        public int Account_To { get; set; }

        public decimal Amount { get; set; }

    }
}
