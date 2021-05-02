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
    public class ItemCollection : ObservableCollection<Item>
    {
        public static ItemCollection GetAllItems()
        {
            ItemCollection itemList = new ItemCollection();
            Item Item = null;

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
                        Item = Item.GetItemFromReader(reader);

                        itemList.Add(Item);
                    }
                }
            }
            return itemList;
        }
    }
}
