using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Interfaces;
using DataTemplateLibrary.Models;
using MySql.Data.MySqlClient;

namespace DataAccessLibrary.DAOS
{
    public class ServiceDAO : AbstractDAO<DBService>, IDAO<DBService>
    {
        private static string table_n = "Services";
        private String C_CREATE = $"INSERT INTO {table_n} (name, hashedPassword, current_credits, isAdmin) VALUES (@name, @hashedPassword, @current_credits, @isAdmin)";
        private String C_UPDATE = $"UPDATE {table_n} SET name = @name, hashedPassword = @hashedPassword, current_credits = @current_credits, WHERE id = @id";
        private String C_READ_ALL = $"SELECT * FROM {table_n}";
        private String C_READ_BY_ID = $"SELECT * FROM {table_n} WHERE id = @id";
        private String C_DELETE = $"DELETE FROM {table_n} WHERE id = @id";
        private String C_GET_BY_NAME = $"SELECT * FROM {table_n} WHERE name = @name";
        public int Create(DBService element)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<DBService> GetAll()
        {
            throw new NotImplementedException();
        }

        public DBService? GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(DBService element)
        {
            throw new NotImplementedException();
        }

        protected override DBService Map(MySqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected override List<MySqlParameter> Map(DBService obj)
        {
            throw new NotImplementedException();
        }
    }
}
