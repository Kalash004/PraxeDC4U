using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.DAOS;
using DataTemplateLibrary.Models;
using Org.BouncyCastle.Security;
using SessionService;
using SessionService.SessionTemplate_Creater;

namespace DataTemplateLibrary
{
    public class DBManager
    {
        private DBUserManager userManager = new DBUserManager();

        /// <summary>
        /// Saves user to database and creates and retrievs his id
        /// </summary>
        /// <param name="user">User to save</param>
        /// <returns>Original user with updated id</returns>
        public DBUser? SingUpUser(DBUser user)
        {
            // return userManager.SingUpUser(user);
            var data = userManager.SingUpUser(user);
            if (data != null) return data.Result;
            else throw new Exception(data.Message);
        }

        /// <summary>
        /// Reads database and returns a user with specific name
        /// </summary>
        /// <param name="name">Name of the user you want to read</param>
        /// <returns>User from database with the specified name</returns>
        public DBUser? ReadUserByName(string name)
        {
            return userManager.GetUserByName(name);
        }

        public ReturnData<bool, DBUser> LogUserIn(DBUser user)
        {
            var data = userManager.LogUserIn(user);
            var user_from_db = data.Result;
            if (user_from_db != null) return new ReturnData<bool, DBUser>(true, user_from_db);
            else return new ReturnData<bool, DBUser>(false, null);
        }
        public ReturnData<SessionId,DBUser> LogUserInCreateSession(DBUser user)
        {
            var returned_user_data = LogUserIn(user);
            if (returned_user_data.Result)
            {
                var sessionId = ServerSideSessionSaverService.GetInstance().AddSession(returned_user_data.Message);
                return new ReturnData<SessionId, DBUser>(sessionId,returned_user_data.Message);
            }
            else throw new Exception("User wasnt found in the database");
        }

    }
}
