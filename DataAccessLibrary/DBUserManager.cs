using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.DAOS;
using DataTemplateLibrary.Models;

namespace DataTemplateLibrary
{
    public class DBUserManager
    {
        UsersDAO usersDAO = new UsersDAO();

        public ReturnData<DBUser?,string> SingUpUser(DBUser user)
        {
            if (usersDAO.GetByName(user) != null)
            {
                return new ReturnData<DBUser?, string>(null,"User already exists in the database");
            }
            int id = usersDAO.Create(user);
            user.ID = id;
            return new ReturnData<DBUser?, string>(user,"Signed up");
        }

        public DBUser? GetUserByName(string name)
        {
            DBUser returned = usersDAO.GetByName(name);
            if (returned.ID > 0)
            {
                return returned;
            }
            else return null;
        }

        public void RemoveUser(DBUser user)
        {
            usersDAO.Delete(user.ID);
        }

        internal ReturnData<DBUser?,string> LogUserIn(DBUser hypothetical_user)
        {
            DBUser user_in_db = GetUserByName(hypothetical_user.Name);
            // CHECK : Might get error instead of null from db if user is null
            if (user_in_db == null) return new ReturnData<DBUser?,string>(null,"User doesnt exist in database");
            // CHECK : If db send all lower case data or not
            if (!user_in_db.HashedPassword.ToLower().Equals(hypothetical_user.HashedPassword.ToLower())) return new ReturnData<DBUser?,string>(null,"Wrong password");
            return new ReturnData<DBUser?, string>(user_in_db,"Welcome");
        }
    }
}
