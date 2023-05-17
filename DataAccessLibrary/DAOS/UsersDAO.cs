using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using MySql.Data.MySqlClient;

namespace DataAccessLibrary.DAOS
{
    internal class UsersDAO : AbstractDAO<User>, IDAO<User>
    {
        private static string table_n = "Users";
        private String C_CREATE = $"INSERT INTO {table_n} (name, hashedPassword, current_credits) VALUES (@name, @hashedPassword, @current_credits)";
        private String C_UPDATE = $"UPDATE {table_n} SET name = @name, hashedPassword = @hashedPassword, current_credits = @current_credits, WHERE id = @id";
        private String C_READ_ALL = $"SELECT * FROM {table_n}";
        private String C_READ_BY_ID = $"SELECT * FROM {table_n} WHERE id = @id";
        private String C_DELETE = $"DELETE FROM {table_n} WHERE id = @id";
        private String C_GET_BY_NAME = $"SELECT * FROM {table_n} WHERE name = @name";

        public int Create(User element)
        {
            return Create(C_CREATE, element);
        }

        public void Delete(int id)
        {
            Delete(C_DELETE, id);
        }

        public List<User> GetAll()
        {
            return GetAll(C_READ_ALL);
        }

        public User? GetByID(int id)
        {
            return GetByID(C_READ_BY_ID, id);
        }

        public void Save(User element)
        {
            Update(C_UPDATE, element, element.ID);
        }

        public User GetByName(string name)
        {
           return GetByName(C_GET_BY_NAME,"@name",name);
        }

        public User GetByName(User element)
        {
            return GetByName(element.Name);
        }

        protected override User Map(MySqlDataReader reader)
        {
            return new User(
                Convert.ToInt32(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                Convert.ToInt32(reader[3].ToString())
            );
        }

        protected override List<MySqlParameter> Map(User obj)
        {
            return new List<MySqlParameter>()
            {
                new MySqlParameter("@name",obj.Name),
                new MySqlParameter("@hashedPassword",obj.HashedPassword),
                new MySqlParameter("@current_credits",obj.CurrentCredits)
            };
        }
    }
}
