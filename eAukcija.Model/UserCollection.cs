using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAukcija.Model
{
    public class UserCollection : ObservableCollection<User>
    {
        public static UserCollection GetAllUsers()
        {
            UserCollection userList = new UserCollection();
            User User = null;

            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ToString();
                sqlConnection.Open();

                string command = "SELECT * FROM Items WHERE IsDeleted = 0 AND IsSold = 0";
                var sqlCommand = new SqlCommand(command, sqlConnection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User = User.GetUserFromReader(reader);

                        userList.Add(User);
                    }
                }
            }
            return userList;
        }
    }
}
