using DataAccessLibrary;
using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;
using SessionService;
using SessionService.SessionTemplate_Creater;

namespace ServerManager
{
    public class ServerManager
    {
        private DBManager dBManager = DBManager.GetInstance();
        private ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();

        /// <summary>
        /// Checks if user exists and credentials are right, asks for a new session id and puts it into the session manager
        /// </summary>
        /// <param name="user">Hypothetical user to check if the credentials are right</param>
        /// <returns>Session id in Result and User from database in Message</returns>
        /// <exception cref="Exception"></exception>
        public ReturnData<SessionId, DBUser> LogUserInCreateSession(DBUser user)
        {
            var returned_user_data = dBManager.LogUserIn(user);
            if (returned_user_data.Result)
            {
                var sessionId = sessionManager.AddSession(returned_user_data.Message,returned_user_data.Message.ID);
                return new ReturnData<SessionId, DBUser>(sessionId, returned_user_data.Message);
            }
            else throw new Exception("User wasnt found in the database");
        }

        /// <summary>
        /// Adds service to bd with id of the user
        /// </summary>
        /// <param name="id">Session id for ServerSideSessionSaverService</param>
        /// <param name="service">Service you want to save without userId</param>
        /// <returns>Returns service with id from db</returns>
        /// <exception cref="Exception">If session doesnt exists</exception>
        public DBService? CreateService(SessionId sessionId, DBService service) { 
            if (sessionManager.SessionExists(sessionId))
            {
                int userId = sessionManager.GetUserFromSessionId(sessionId);
                service.UserId = userId;
                return dBManager.CreateService(service,userId);
            }
            else
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
                return dBManager.GetAllUserServices(sessionManager.GetUserFromSessionId(sessionId));
            }
            else
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
                return dBManager.GetServiceFromDB(sessionManager.GetUserFromSessionId(sessionId).ID, serviceId);
            }
            else
            {
                throw new Exception($"No Session with : {sessionId} session id");
            }
        }

        /// <summary>
        /// Update service on db using service id and session id
        /// </summary>
        /// <param name="sessionId">Current user session id</param>
        /// <param name="serviceId">Service id of the service to be updated</param>
        /// <param name="updatedService">New service with updated values</param>
        /// <exception cref="Exception"></exception>
        public void UpdateService(SessionId sessionId, int serviceId, DBService updatedService)
        {
            ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();
            if (sessionManager.SessionExists(sessionId))
            {
                updatedService.ID = serviceId;
                dBManager.UpdateService(serviceId, updatedService);
            }
            else
            {
                throw new Exception($"No Session with : {serviceId} session id");
            }
        }

        public DBUser? SingUpUser(DBUser user)
        {
            return dBManager.SingUpUser(user);
        }

        public DBUser? ReadUserByName(string name)
        {
            return dBManager.ReadUserByName(name);
        }


    }
}