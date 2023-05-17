using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DAOS;
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public class DBUserManager
    {
        UsersDAO usersDAO = new UsersDAO();

        public DBUser SingUpUser(DBUser user)
        {
            if (usersDAO.GetByName(user) != null)
            {
                throw new Exception("User name already exists in database");
            }
            int id = usersDAO.Create(user);
            user.ID = id;
            return user;
        }

        public DBUser GetUserByName(string name)
        {
            DBUser returned = usersDAO.GetByName(name);
            return returned;
        }

        public void RemoveUser(DBUser user)
        {
            usersDAO.Delete(user.ID);
        }
    }
}
