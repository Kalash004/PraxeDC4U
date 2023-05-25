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
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="serviceId"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public int GetBoughtAmountFromService(SessionId sessionId, int serviceId,EnumAnaliticsTimeSpan timeSpan)
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            dbManager.ServiceExists(serviceId);
            // check if user owns the service
            dbManager.UserOwnsService(userId, serviceId);
            return dbManager.GetBoughtAmount(userId, serviceId);
        }

        public DBUser GetUserBySessionId(SessionId sessionId)
        {
            CheckSessionExistance(sessionId);
            return dbManager.GetUser(sessionManager.GetUserIdFromSessionId(sessionId));
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
            if (user.CurrentCredits < transaction.Amount) throw new Exception($"User doesnt have enough credits to send the transaction {user.CurrentCredits} / {transaction.Amount}");
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

        /// <summary>
        /// Creates a transaction and updates money on buyer side.
        /// Transaction sender is admin.
        /// </summary>
        /// <param name="sessionId">User session id</param>
        /// <param name="amount">Amount of buying</param>
        /// <returns></returns>
        public DBUser? BuyCredits(SessionId sessionId, int amount)
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            // Money API logic (not adding)
            DBUser? updatedUser = dbManager.AddCreditsToUser(userId, amount);
            return updatedUser;
        }

        // Methods that work with user :

        public DBUser GetUserById(int id)
        {
            return dbManager.GetUser(id);
        }

        public DBUser GetUserByName(string name)
        {
           return dbManager.GetUserByName(name);
        }

        /// <summary>
        /// Checks if user exists and credentials are right, asks for a new session id and puts it into the session manager
        /// </summary>
        /// <param name="user">Hypothetical user to check if the credentials are right</param>
        /// <returns>Session id in Result and User from database in Message</returns>
        /// <exception cref="Exception"></exception>
        public SessionId LogUserInCreateSession(DBUser user)
        {
            DBUser returned_user_data = dbManager.LogUserIn(user);
            var sessionId = sessionManager.AddSession(returned_user_data, returned_user_data.ID);
            return sessionId;
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