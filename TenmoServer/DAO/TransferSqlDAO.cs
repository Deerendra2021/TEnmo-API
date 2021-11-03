using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;


namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferSqlDAO
    {
        private readonly string connectionString;

        private readonly string MakeTransferSql =
            "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
            "VALUES " +
            "(1001, " +
            "2001, " +
            "(SELECT account_id FROM accounts a INNER JOIN users u ON u.user_id = a.user_id WHERE u.user_id = @fromUserId), " +
            "(SELECT account_id FROM accounts a INNER JOIN users u ON u.user_id = a.user_id WHERE u.user_id = @toUserId), " +
            "@amount) " +
            "SELECT @@IDENTITY; " +
            "UPDATE accounts SET balance -= @amount WHERE user_id = @fromUserId; " +
            "UPDATE accounts SET balance += @amount WHERE user_id = @toUserId; ";


        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Transfers MakeTransfer(Transfers newTransfer)
        {
            /*Transfers transfer = new Transfers
            {
                Account_From = newTransfer.Account_From,
                Account_To = newTransfer.Account_To,
                Amount = newTransfer.Amount
            };*/

            

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(MakeTransferSql, conn);
                cmd.Parameters.AddWithValue("@fromUserId", newTransfer.Account_From);
                cmd.Parameters.AddWithValue("@toUserId", newTransfer.Account_To);
                cmd.Parameters.AddWithValue("@amount", newTransfer.Amount);

                newTransfer.Transfer_Id = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT transfer_type_id, transfer_status_id FROM transfers WHERE transfer_id = @newTransferId", conn);
                cmd.Parameters.AddWithValue("@newTransferId", newTransfer.Transfer_Id);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    newTransfer.Transfers_Type_Id = Convert.ToInt32(reader["transfer_type_id"]);
                    newTransfer.Transfers_Status_Id = Convert.ToInt32(reader["transfer_status_id"]);
                }
            }
            return newTransfer;
        }



    }
}
