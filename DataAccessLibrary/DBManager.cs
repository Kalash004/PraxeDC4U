using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;
using Org.BouncyCastle.Security;
using SessionService;
using SessionService.SessionTemplate_Creater;

namespace DataAccessLibrary
{
    public class DBManager
    {
        private DBUserManager userManager = new DBUserManager();
        private DBServiceManager serviceManager = new DBServiceManager();   
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
        /// <summary>
        /// Check if user exists in the database and credentials
        /// </summary>
        /// <param name="user">Hypothetical user to check he exists in the database and if password is same</param>
        /// <returns>True in result if user exists and credentials are right, User from database.</returns>
        private ReturnData<bool, DBUser?> LogUserIn(DBUser user)
        {
            var data = userManager.LogUserIn(user);
            var user_from_db = data.Result;
            if (user_from_db != null) return new ReturnData<bool, DBUser?>(true, user_from_db);
            else return new ReturnData<bool, DBUser?>(false, null);
        }
        /// <summary>
        /// Checks if user exists and credentials are right, asks for a new session id and puts it into the session manager
        /// </summary>
        /// <param name="user">Hypothetical user to check if the credentials are right</param>
        /// <returns>Session id in Result and User from database in Message</returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// Creates Service
        /// </summary>
        /// <param name="service">Service with userId inside</param>
        /// <returns>Service with id from db</returns>
        private DBService? CreateService(DBService service)
        {
            return serviceManager.CreateService(service).Result;
        }

        /// <summary>
        /// Adds service to bd with id of the user
        /// </summary>
        /// <param name="id">Session id for ServerSideSessionSaverService</param>
        /// <param name="service">Service you want to save without userId</param>
        /// <returns>Returns service with id from db</returns>
        /// <exception cref="Exception">If session doesnt exists</exception>
        public DBService? CreateService(SessionId sessionId, DBService service)
        {
            ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();
            if (sessionManager.SessionExists(sessionId))
            {
                service.UserId = sessionManager.GetUserFromSessionId(sessionId).ID;
                return CreateService(service);
            } else
            {
                throw new Exception($"No Session with : {sessionId} session id");
            }
        }

        /// <summary>
        /// Gets all user services from database
        /// </summary>
        /// <param name="sessionId">Session id of the user</param>
        /// <returns>List of services with their ids from db </returns>
        /// <exception cref="Exception">If session wasnt found</exception>
        public List<DBService?> GetAllUserServices(SessionId sessionId)
        {
            ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();
            if (sessionManager.SessionExists(sessionId))
            {
                return serviceManager.GetAllServiceByUser(sessionManager.GetUserFromSessionId(sessionId));
            } else
            {
                throw new Exception($"No Session with : {sessionId} session id");
            }
        }
        /// <summary>
        /// Gets concrete service from db using session id and service id
        /// </summary>
        /// <param name="sessionId">Session id of the user</param>
        /// <param name="serviceId">ID of service from db</param>
        /// <returns>One service from db</returns>
        /// <exception cref="Exception">If no session was found</exception>
        public DBService? GetServiceFromDB(SessionId sessionId, int serviceId)
        {
            ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();
            if (sessionManager.SessionExists(sessionId))
            {
                return serviceManager.GetOneServiceByUserAndId(sessionManager.GetUserFromSessionId(sessionId), serviceId);
            } else
            {
                throw new Exception($"No Session with : {sessionId} session id");
            }
        }
        /// <summary>
        /// Update service on db using service id and session id
        /// </summary>
        /// <param name="sessionId">Current user session id</param>
        /// <param name="serviceId">Service id of the service to be updated </param>
        /// <param name="updatedService">New service with updated values</param>
        /// <exception cref="Exception"></exception>
        public void UpdateService(SessionId sessionId, int serviceId, DBService updatedService)
        {
            ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();
            if (sessionManager.SessionExists(sessionId))
            {
                serviceManager.UpdateService(serviceId, updatedService);
            } else
            {
                throw new Exception($"No Session with : {serviceId} session id");
            }
        }
    }

    
}
