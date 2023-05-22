using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Interfaces;
using DataTemplateLibrary.Models;
using MySql.Data.MySqlClient;

namespace DataAccessLibrary.DAOS
{

    internal class TransactionDAO : AbstractDAO<DBTransaction>, IDAO<DBTransaction>
    {
        private static string table_n = "transactions";
        private String C_CREATE = $"INSERT INTO {table_n} (user_id, ser_name, current_price, creation,`update`, isShown, short_description, long_description, link, isDeleted) VALUES (@user_id, @ser_name, @current_price, @creation, @update, @isShown, @short_description, @long_description, @link, @isDeleted)";
        private String C_UPDATE = $"UPDATE {table_n} SET user_id = @user_id, ser_name = @ser_name, current_price = @current_price, creation = @creation, `update` = @update, isShown = @isShown, short_description = @short_description, long_description = @long_description, link = @link, isDeleted = @isDeleted WHERE id = @id";
        private String C_READ_ALL = $"SELECT * FROM {table_n}";
        private String C_READ_BY_ID = $"SELECT * FROM {table_n} WHERE id = @id";
        private String C_DELETE = $"DELETE FROM {table_n} WHERE id = @id";
        private String C_GET_BY_USER_ID = $"SELECT * FROM {table_n} WHERE user_id = @user_id";
        private List<string> atributeList = new List<string>() { "id", "reciever_id", "sender_id", "send_date", "send_time", "cost", "service_id" };
        public int Create(DBTransaction element)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<DBTransaction> GetAll()
        {
            throw new NotImplementedException();
        }

        public DBTransaction? GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(DBTransaction element)
        {
            throw new NotImplementedException();
        }

        protected override DBTransaction Map(MySqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected override List<MySqlParameter> Map(DBTransaction obj)
        {
            throw new NotImplementedException();
        }
    }
}
