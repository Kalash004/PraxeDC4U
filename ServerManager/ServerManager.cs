using DataAccessLibrary;
using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;
using SessionService;
using SessionService.SessionTemplate_Creater;

namespace ServerManagement
{
    public class ServerManager
    {
        private DBManager dbManager = DBManager.GetInstance();
        private ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();

        /// <summary>
        /// Checks if user exists and credentials are right, asks for a new session id and puts it into the session manager
        /// </summary>
        /// <param name="user">Hypothetical user to check if the credentials are right</param>
        /// <returns>Session id in Result and User from database in Message</returns>
        /// <exception cref="Exception"></exception>
        public ReturnData<SessionId, DBUser> LogUserInCreateSession(DBUser user)
        {
            var returned_user_data = dbManager.LogUserIn(user);
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
                return dbManager.CreateService(service,userId);
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
            if (sessionManager.SessionExists(sessionId))
            {
                return dbManager.GetAllUserServices(sessionManager.GetUserFromSessionId(sessionId));
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
            if (sessionManager.SessionExists(sessionId))
            {
                return dbManager.GetServiceFromDB(sessionManager.GetUserFromSessionId(sessionId), serviceId);
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
            if (sessionManager.SessionExists(sessionId))
            {
                updatedService.ID = serviceId;
                dbManager.UpdateService(serviceId, updatedService);
            }
            else
            {
                throw new Exception($"No Session with : {serviceId} session id");
            }
        }

        public DBUser? SingUpUser(DBUser user)
        {
            return dbManager.SingUpUser(user);
        }

        public DBUser? ReadUserByName(string name)
        {
            return dbManager.ReadUserByName(name);
        }
        /// <summary>
        /// Creates a transaction for database, and sets values to users cretids
        /// </summary>
        /// <param name="sessionId">Session id of the user who sends the money</param>
        /// <param name="transaction">Transaction data</param>
        /// <returns></returns>
        public bool CreateTransaction(SessionId sessionId, DBTransaction transaction, int recieverId)
        {
            if (!sessionManager.SessionExists(sessionId)) throw new Exception($"Session with {sessionId} session id doesnt exist");
            int userId = sessionManager.GetUserFromSessionId(sessionId);

            if (!dbManager.UserExists(userId)) throw new Exception($"Sending user doesnt exist in the database, cant create a transaction from non existing user");
            if (!dbManager.UserExists(recieverId)) throw new Exception("User doesnt exist in database, cant create a transaction to a none existing user");
            DBUser user = dbManager.GetUser(userId);
            DBUser recievingUser = dbManager.GetUser(recieverId);

            if (user.CurrentCredits < transaction.Amount) throw new Exception($"User doesnt have enoug credits to send the transaction {user.CurrentCredits} / {transaction.Amount}");
            // TODO: Create Transaction (if disconnected - rollback)!!!
            user.CurrentCredits -= transaction.Amount;
            recievingUser.CurrentCredits += transaction.Amount;
            dbManager.UpdateUser(userId, user);
            dbManager.UpdateUser(recieverId, recievingUser);

        }

    }
}