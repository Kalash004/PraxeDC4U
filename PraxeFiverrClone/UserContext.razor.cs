using CookieService;
using DataTemplateLibrary.Models;
using ServerManagement;
using SessionService;
using SessionService.SessionTemplate_Creater;

namespace PraxeFiverrClone
{
    public class UserContext
    {
        public CookieManager? CookieManager { get; set; }
        public ServerManager? ServerManager { get; set; }
        public string SessionID { get; private set; } = "";
        public int UserID {
            get
            {
                try
                {
                    return ServerSideSessionSaverService.GetInstance().GetUserFromSessionId(new SessionService.SessionTemplate_Creater.SessionId(SessionID));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return -1;
            }
        }
        public UserContext(CookieManager cookieManager, ServerManager serverManager) 
        { 
            CookieManager = cookieManager;
            ServerManager = serverManager;
        }

        public async Task Login(DBUser user)
        {
            if(await IsLoggedIn())
            {
                return;
            }
            if(ServerManager == null)
            {
                return;
            }

            try
            {
                string sessionID = ServerManager.LogUserInCreateSession(user).Result.Id;
                SetCurrentSession(sessionID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Logout()
        {
            SetCurrentSession("");    
        }
        public async Task<bool> IsLoggedIn()
        {
            bool loginStatus = false;
            if (CookieManager != null)
            {
                string sessionID = await CookieManager.GetSessionCookieString("sessionID");
                if (CheckIfSessionExists(sessionID))
                {
                    SessionID = sessionID;
                    loginStatus = true;
                }
                else
                {
                    SetCurrentSession("");
                }
            }
            return loginStatus;
        }

        private static bool CheckIfSessionExists(string sessionID) {
            SessionId sessionId = new(sessionID);
            try{
                if (ServerSideSessionSaverService.GetInstance().SessionExists(sessionId))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return false;
        }

        private void SetCurrentSession(string sessionID)
        {
            SessionID = sessionID;
            CookieManager?.SetSessionCookie("sessionID", sessionID);
        }
    }
}
