using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        // (data) values (@data)
        private static String C_CREATE = $"INSERT INTO {table_n}";
        // data = @data,
        private static String C_UPDATE = $"UPDATE {table_n} SET WHERE id = @id";
        private static String C_READ_ALL = $"SELECT * FROM {table_n}";
        private static String C_READ_BY_ID = $"SELECT * FROM {table_n} WHERE id = @id";
        private static String C_DELETE = $"DELETE FROM {table_n} WHERE id = @id";
        private static String C_GET_BY_USER_ID = $"SELECT * FROM {table_n} WHERE user_id = @user_id";

        private static List<string> atributeList = new List<string>() { "id", "reciever_id", "sender_id", "send_date", "send_time", "cost", "service_id" };

        public TransactionDAO()
        {
            SetSQL(atributeList);
        } 
        private static void SetSQL(List<string> atributes)
        {
            SetSQLCreate(atributes);
            SetSQLUpdate(atributes);
        }

        private static void SetSQLUpdate(List<string> atributes)
        {
            string last = atributes.Last();
            foreach (var atribute in atributes)
            {
                if (atribute == last)
                {
                    C_UPDATE += $"{atribute} = @{atribute},";
                }
                else
                {
                    C_UPDATE += $"{atribute} = @{atribute},";
                }
            }
        }

        private static void SetSQLCreate(List<string> atributes)
        {
            C_CREATE += "(";
            string last = atributes.Last();
            foreach (var atribute in atributes)
            {
                if (atribute == last)
                {
                    C_CREATE += $"{atribute}";
                }
                else
                {
                    C_CREATE += $"{atribute},";
                }
            }
            C_CREATE += ") values (";
            foreach (var atribute in atributes)
            {
                if (atribute == last)
                {
                    C_CREATE += $"@{atribute}";
                }
                else
                {
                    C_CREATE += $"@{atribute},";
                }
            }
            C_CREATE += ")";
        }

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
