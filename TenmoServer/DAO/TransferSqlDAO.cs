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

        private readonly string ViewTransferSql =
            "SELECT " +
            "transfer_id, " +
            "transfer_type_id, " +
            "transfer_status_id, " +
            "account_from, " +
            "u1.username AS accountFromUserName, " +
            "u1.user_id AS accountFromUserId, " +
            "account_to, " +
            "u2.username AS accountToUserName, " +
            "u2.user_id AS accountToUserId, " +
            "amount " +
            "FROM transfers t LEFT OUTER JOIN accounts acf ON acf.account_id = t.account_from " +
            "LEFT OUTER JOIN accounts act ON act.account_id = t.account_to " +
            "INNER JOIN users u1 ON u1.user_id = acf.user_id " +
            "INNER JOIN users u2 ON u2.user_id = act.user_id " +
            "WHERE t.account_from = (SELECT account_id FROM accounts acf LEFT OUTER JOIN users u ON u.user_id = acf.user_id WHERE u.user_id = @user_id) " +
            "OR t.account_to = (SELECT account_id FROM accounts act LEFT OUTER JOIN users u ON u.user_id = act.user_id WHERE u.user_id = @user_id)";


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
                cmd.Parameters.AddWithValue("@fromUserId", newTransfer.AccountFromUserId);
                cmd.Parameters.AddWithValue("@toUserId", newTransfer.AccountToUserId);
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

        public List<Transfers> ViewTransfers(int userId)
        {
            List<Transfers> userTransfers = new List<Transfers>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(ViewTransferSql, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Transfers transfer = new Transfers() 
                    { 
                        Transfer_Id = Convert.ToInt32(reader["transfer_id"]),
                        Transfers_Type_Id = Convert.ToInt32(reader["transfer_type_id"]),
                        Transfers_Status_Id = Convert.ToInt32(reader["transfer_status_id"]),
                        Account_From = Convert.ToInt32(reader["account_from"]),
                        AccountFromUserName = "From: " + Convert.ToString(reader["accountFromUserName"]),
                        AccountFromUserId = Convert.ToInt32(reader["accountFromUserId"]),
                        Account_To = Convert.ToInt32(reader["account_to"]),
                        AccountToUserName = "To: " + Convert.ToString(reader["accountToUserName"]),
                        AccountToUserId = Convert.ToInt32(reader["accountToUserId"]),
                        Amount = Convert.ToDecimal(reader["amount"])
                    };
                    userTransfers.Add(transfer);
                }
            }
            return userTransfers;
        }



    }
}
