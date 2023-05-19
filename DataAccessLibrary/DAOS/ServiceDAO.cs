using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
        private static string table_n = "services";
        private String C_CREATE = $"INSERT INTO {table_n} (user_id, ser_name, current_price, creation,`update`, isShown, short_description, long_description, link) VALUES (@user_id, @ser_name, @current_price, @creation, @update, @isShown, @short_description, @long_description, @link)";
        private String C_UPDATE = $"UPDATE {table_n} SET user_id = @user_id, ser_name = @ser_name, current_price = @current_price, creation = @creation, `update` = @update, isShown = @isShown, short_description = @short_description, long_description = @long_description, link = @link WHERE id = @id";
        private String C_READ_ALL = $"SELECT * FROM {table_n}";
        private String C_READ_BY_ID = $"SELECT * FROM {table_n} WHERE id = @id";
        private String C_DELETE = $"DELETE FROM {table_n} WHERE id = @id";
        private String C_GET_BY_USER_ID = $"SELECT * FROM {table_n} WHERE user_id = @user_id";
        public int Create(DBService element)
        {
            return Create(C_CREATE, element);
        }

        public void Delete(int id)
        {
            Delete(C_DELETE, id);
        }

        public List<DBService> GetAll()
        {
            return GetAll(C_READ_ALL);
        }

        public DBService? GetByID(int id)
        {
            return GetByID(C_READ_BY_ID, id);
        }

        public void Save(DBService element)
        {
            Update(C_UPDATE, element, element.ID);
        }
        //TODO : TEST THIS !
        public List<DBService> GetAllByUserID(int id)
        {
            return Get(C_GET_BY_USER_ID, new List<MySqlParameter>()
            {
                new MySqlParameter("@user_id",id)
            });
        }

        protected override DBService Map(MySqlDataReader reader)
        {
            int Id = Convert.ToInt32(reader[0].ToString());
            int userId = Convert.ToInt32(reader[1].ToString());
            string serviceName = reader[2].ToString();
            int currentPrice = Convert.ToInt32(reader[3].ToString());
            DateOnly created = DateOnly.FromDateTime(DateTime.Parse(reader[4].ToString()));
            var test = reader[5].ToString();
            DateOnly? update;
            if (test != "")
            {
                update = DateOnly.FromDateTime(DateTime.Parse(reader[5].ToString()));
            }
            else
            {
                update = null;
            }
            bool isVisible = Convert.ToBoolean(Convert.ToInt32(reader[6].ToString()));
            string shortDescript = reader[7].ToString();
            string? longDescript = reader[8].ToString();
            string? linkToImage = reader[9].ToString();

            return new DBService(
                Id,
                userId,
                serviceName,
                currentPrice,
                created,
                update,
                isVisible,
                shortDescript,
                longDescript,
                linkToImage
                );
        }

        protected override List<MySqlParameter> Map(DBService obj)
        {
            if (obj.Updated == null)
            {
                return new List<MySqlParameter>()
                {
                new MySqlParameter("@user_id",obj.UserId),
                new MySqlParameter("@ser_name",obj.ServiceName),
                new MySqlParameter("@current_price",obj.CurrentPrice),
                new MySqlParameter("@creation",obj.Created.ToString("o",CultureInfo.InvariantCulture)),
                new MySqlParameter("@update",obj.Updated),
                new MySqlParameter("@isShown",obj.IsShown),
                new MySqlParameter("@short_description",obj.ShortDescription),
                new MySqlParameter("@long_description",obj.LongDescription),
                new MySqlParameter("@link",obj.LinkToImage)
                };
            }
            else
            {
                return new List<MySqlParameter>()
                {
                new MySqlParameter("@user_id",obj.UserId),
                new MySqlParameter("@ser_name",obj.ServiceName),
                new MySqlParameter("@current_price",obj.CurrentPrice),
                new MySqlParameter("@creation",obj.Created.ToString("o",CultureInfo.InvariantCulture)),
                new MySqlParameter("@update",((DateOnly)obj.Updated).ToString("o",CultureInfo.InvariantCulture)),
                new MySqlParameter("@isShown",obj.IsShown),
                new MySqlParameter("@short_description",obj.ShortDescription),
                new MySqlParameter("@long_description",obj.LongDescription),
                new MySqlParameter("@link",obj.LinkToImage)
                };
            }
        }
    }
}
