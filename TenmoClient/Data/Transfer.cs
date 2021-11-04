using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class Transfer
    {
        public int transfer_Id { get; set; }
                   
        public int transfers_Type_Id { get; set; }
                   
        public int transfers_Status_Id { get; set; }

        public int account_From { get; set; }

        public int account_To { get; set; }

        public decimal amount { get; set; }

    }
}
