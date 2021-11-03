using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountSqlDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public decimal GetAccountBalance(int userId)
        {
            decimal accountBalance;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts WHERE user_id = @id", conn);

                cmd.Parameters.AddWithValue("@id", userId);

                accountBalance = Convert.ToDecimal(cmd.ExecuteScalar());

            }
            return accountBalance;

        }
    }
}
