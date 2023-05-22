using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;
using Org.BouncyCastle.Security;

namespace DataAccessLibrary
{
    /// <summary>
    /// Cascade class for working with database and Session Service
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    public class DBManager
    {
        private readonly DBUserManager userManager = new();
        private readonly DBServiceManager serviceManager = new();

        /// <summary>
        /// Saves user to database and creates and retrievs his id
        /// </summary>
        /// <param name="user">User to save</param>
        /// <returns>Original user with updated id</returns>
        public DBUser? SingUpUser(DBUser user)
        {
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

        /// <summary>
        /// Check if user exists in the database and credentials
        /// </summary>
        /// <param name="user">Hypothetical user to check he exists in the database and if password is same</param>
        /// <returns>True in result if user exists and credentials are right, User from database.</returns>
        public ReturnData<bool, DBUser?> LogUserIn(DBUser user)
        {
            var data = userManager.LogUserIn(user);
            var user_from_db = data.Result;
            if (user_from_db != null) return new ReturnData<bool, DBUser?>(true, user_from_db);
            else return new ReturnData<bool, DBUser?>(false, null);
        }

        /// <summary>
        /// Creates Service
        /// </summary>
        /// <param name="service">New service</param>
        /// <param name="userId">User id for service</param>
        /// <returns>Service with id from db</returns>
        public DBService? CreateService(DBService service, int userId)
        {
            service.UserId = userId;
            return serviceManager.CreateService(service).Result;
        }


        public DBService? GetServiceFromDB(int userId, int serviceId)
        {
            return serviceManager.GetOneServiceByUserIdAndServiceId(userId, serviceId);
        }

        public void UpdateService(int serviceId, DBService updatedService)
        {
            updatedService.ID = serviceId;
            serviceManager.UpdateService(serviceId, updatedService);
        }

        public List<DBService?> GetAllUserServices(int userId)
        {
            return serviceManager.GetAllServiceByUser(userId);
        }

    }
}
