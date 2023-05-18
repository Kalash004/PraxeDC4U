using MySqlConnector;

namespace PraxeFiverrClone.Data
{
    public class DBConnection
    {
        public static string? ConStr { get; set; }
        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new(ConStr);
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
