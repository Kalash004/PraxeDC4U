using DataAccessLibrary;
using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;
using SessionService;
using SessionService.SessionTemplate_Creater;

namespace ServerManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerManager
    {
        private DBManager dbManager = DBManager.GetInstance();
        private ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();


        // Methods that work with session id as an atribute:

        /// <summary>
        /// Adds service to bd with id of the user
        /// </summary>
        /// <param name="id">Session id for ServerSideSessionSaverService</param>
        /// <param name="service">Service you want to save without userId</param>
        /// <returns>Returns service with id from db</returns>
        /// <exception cref="Exception">If session doesnt exists</exception>
        public DBService? CreateService(SessionId sessionId, DBService service)
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            service.UserId = userId;
            return dbManager.CreateService(service, userId);
        }

        /// <summary>
        /// Gets all user services from database
        /// </summary>
        /// <param name="sessionId">Session id of the user</param>
        /// <returns>List of services with their ids from db </returns>
        /// <exception cref="Exception">If session wasnt found</exception>
        public List<DBService?> GetAllUserServices(SessionId sessionId)
        {
            CheckSessionExistance(sessionId);
            return dbManager.GetAllUserServices(sessionManager.GetUserIdFromSessionId(sessionId));
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
            CheckSessionExistance(sessionId);
            return dbManager.GetServiceFromDB(sessionManager.GetUserIdFromSessionId(sessionId), serviceId);
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
            CheckSessionExistance(sessionId);
            updatedService.ID = serviceId;
            dbManager.UpdateService(serviceId, updatedService);
        }

        /// <summary>
        /// Creates a transaction for database, and sets values to users cretids
        /// </summary>
        /// <param name="sessionId">Session id of the user who sends the money</param>
        /// <param name="transaction">Transaction data</param>
        /// <param name="recieverId">To be sure that the action has a reciever id</param>
        /// <returns></returns>
        public bool CreateTransaction(SessionId sessionId, DBTransaction transaction, int recieverId)
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            if (!dbManager.UserExists(userId)) throw new Exception($"Sending user doesnt exist in the database, cant create a transaction from non existing user");
            if (!dbManager.UserExists(recieverId)) throw new Exception("User doesnt exist in database, cant create a transaction to a none existing user");
            DBUser user = dbManager.GetUser(userId);
            DBUser recievingUser = dbManager.GetUser(recieverId);
            if (user.CurrentCredits < transaction.Amount) throw new Exception($"User doesnt have enoug credits to send the transaction {user.CurrentCredits} / {transaction.Amount}");
            return dbManager.CreateTransaction(transaction, user.ID, recieverId, transaction.Amount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public int GetUserIdFromSessionId(SessionId sessionId)
        {
            CheckSessionExistance(sessionId);
            return sessionManager.GetUserIdFromSessionId(sessionId);
        }

        public DBUser? BuyCredits(SessionId sessionId,int amount) 
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            // Money API logic (not adding)
            // Create a transaction on the database and insert money
            DBUser? updatedUser = dbManager.AddCreditsToUser(userId, amount);
            // Return updated user 
            return updatedUser;
        }

        // Methods that work with user :

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
                var sessionId = sessionManager.AddSession(returned_user_data.Message, returned_user_data.Message.ID);
                return new ReturnData<SessionId, DBUser>(sessionId, returned_user_data.Message);
            }
            else throw new Exception("User wasnt found in the database");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DBUser? SingUpUser(DBUser user)
        {
            return dbManager.SingUpUser(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DBUser? ReadUserByName(string name)
        {
            return dbManager.ReadUserByName(name);
        }


        // Methods that work with services :

        /// <summary>
        /// This method is used for testing only
        /// </summary>
        /// <returns>ALL services that exist in the database</returns>
        [Obsolete("Method is used for testing purposes, will need a remake in future")]
        public List<DBService?> GetAllServices()
        {
            return dbManager.GetAllServices();
        }

        // Private Methods

        private void CheckSessionExistance(SessionId sessionId)
        {
            if (!sessionManager.SessionExists(sessionId)) throw new Exception($"Session with {sessionId} session id doensnt exist");
        }
    }
}