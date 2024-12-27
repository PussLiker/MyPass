using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using mypass.ViewModel;
namespace mypass.Model
{
    internal class AllPasses
    {
        MainAuthWindowVM mainAuthWindowVM = new MainAuthWindowVM();
        public void GetInfo(int IDAccount)
        {
            using (var connection = new SQLiteConnection(mainAuthWindowVM.FullBDPath))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "";
                }
            }
        }
        public void UpdateAccount(Account account, string oldUsername)
        {
            using (var connection = new SQLiteConnection(mainAuthWindowVM.FullBDPath))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE Accounts 
                                    SET ServiceName = @ServiceName, 
                                        URL = @URL, 
                                        LoginAccount = @LoginAccount, 
                                        Password = @Password 
                                    WHERE ServiceName = @OldServiceName;";

                    command.Parameters.AddWithValue("@ServiceName", account.Username);
                    command.Parameters.AddWithValue("@URL", account.URL ?? string.Empty); // Если URL отсутствует, сохраняем пустую строку
                    command.Parameters.AddWithValue("@LoginAccount", account.Email);
                    command.Parameters.AddWithValue("@Password", account.Password);
                    command.Parameters.AddWithValue("@OldServiceName", oldUsername);

                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteAccount(string username)
        {
            using (var connection = new SQLiteConnection(mainAuthWindowVM.FullBDPath))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"DELETE FROM Accounts WHERE ServiceName = @ServiceName;";
                    command.Parameters.AddWithValue("@ServiceName", username);
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
