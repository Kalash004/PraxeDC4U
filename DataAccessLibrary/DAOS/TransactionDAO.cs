using System;
using System.Collections.Generic;
using System.Globalization;
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
        private static String C_GET_BY_SERVICE_ID = $"SELECT * FROM {table_n} WHERE service_id = @service_id";

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
            return Create(C_CREATE, element);
        }

        public void Delete(int id)
        {
            Delete(C_DELETE, id);
        }

        public List<DBTransaction> GetAll()
        {
            return GetAll(C_READ_ALL);
        }

        public DBTransaction? GetByID(int id)
        {
            return GetByID(C_READ_BY_ID, id);
        }

        public void Save(DBTransaction element)
        {
            Update(C_UPDATE, element, element.ID);
        }

        public List<DBTransaction> GetAllByUserId(int userId)
        {
            return Get(C_GET_BY_USER_ID, new List<MySqlParameter> { new MySqlParameter("@user_id", userId) });
        }

        protected override DBTransaction Map(MySqlDataReader reader)
        {
            int id = Convert.ToInt32(reader[0]);
            int recieverId = Convert.ToInt32(reader[1]);
            int senderId = Convert.ToInt32(reader[2]);
            DateOnly transactionDate = DateOnly.FromDateTime(DateTime.Parse(reader[3].ToString()));
            TimeOnly transactionTime = TimeOnly.FromDateTime(DateTime.Parse(reader[4].ToString()));
            DateTime dateTime = transactionDate.ToDateTime(transactionTime);
            int amount = Convert.ToInt32(reader[5]);
            int serviceId = Convert.ToInt32(reader[6]);

            return new DBTransaction(id, recieverId, senderId, dateTime, amount, serviceId);
        }

        protected override List<MySqlParameter> Map(DBTransaction obj)
        {
            List<MySqlParameter> parameters = new List<MySqlParameter>()
            {
                new MySqlParameter("@id",obj.ID),
                new MySqlParameter("@reciever_id",obj.RecievingUserId),
                new MySqlParameter("@sender_id",obj.SendingUserId),
                new MySqlParameter("@send_date",DateOnly.FromDateTime(obj.DateOfTransaction).ToString("o",CultureInfo.InvariantCulture)),
                new MySqlParameter("@send_time",TimeOnly.FromDateTime(obj.DateOfTransaction).ToString("o",CultureInfo.InvariantCulture)),
                new MySqlParameter("@cost",obj.Amount),
                new MySqlParameter("@service_id",obj.ServiceId)
            };
            return parameters;
        }

        public List<DBTransaction> GetAllByServiceId(int serviceId)
        {
            return Get(C_GET_BY_SERVICE_ID, new List<MySqlParameter> { new MySqlParameter("@service_id", serviceId) });
        }
    }
}
