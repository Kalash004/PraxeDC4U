using PraxeFiverrClone.Pages;
using MySqlConnector;
using System.Data;

namespace PraxeFiverrClone.Data
{
    public class DBManager
    {
        public Service[] GetAllServices()
        {
            List<Service> list = new();
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using MySqlCommand cmd = new("sp_GetAllServices", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Service()
                    {
                        Id = reader.GetInt16("id"),
                        Name = reader.GetString("ser_name"),
                        Owner = reader.GetInt16("user_id"),
                        Price = reader.GetInt16("current_price"),
                        Description = reader.GetString("short_description"),
                       
                    });
                }
            }
            return list.ToArray();
        }
    }
}