using CookieService;
using DataTemplateLibrary.Models;
using ServerManagement;
using SessionService;
using SessionService.SessionTemplate_Creater;

namespace LoginService
{
    public class LoginManager
    {
        protected CookieManager? CookieManager { get; set; }
        protected ServerManager? ServerManager { get; set; }

        public string SessionID { get; private set; } = "";

        public LoginManager(CookieManager cookieManager, ServerManager serverManager)
        {
            CookieManager = cookieManager;
            ServerManager = serverManager;

            Initialize();
        }

        private async void Initialize()
        {
            if (CookieManager != null)
            {
                string sessionID = await CookieManager.GetSessionCookieString("sessionID");
                if (CheckIfSessionExists(sessionID))
                {
                    SessionID = sessionID;
                                    }
                else
                {
                    SetCurrentSession("");
                }
            }
        }

        /// <summary>
        /// Returns current signed in user ID.
        /// If no user is signed in the method returns -1.
        /// </summary>
        /// <returns>ID of a signed user.</returns>
        public int GetUserID()
        {
            try
            {
                return ServerSideSessionSaverService.GetInstance().GetUserIdFromSessionId(new SessionId(SessionID));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }

        /// <summary>
        /// Logs in the user usig it's credentials.
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="LoginSignupException"></exception>
        public async Task Login(DBUser user)
        {
            if (await IsLoggedIn())
            {
                throw new LoginSignupException("The user is already logged in");
            }
            if (ServerManager == null)
            {
                throw new LoginSignupException("The ServerManager is null");
            }

            try
            {
                string sessionID = ServerManager.LogUserInCreateSession(user).Id;
                SetCurrentSession(sessionID);
            }
            catch
            {
                throw new LoginSignupException("Failed to create session");
            }
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        public void Logout()
        {
            ServerSideSessionSaverService.GetInstance().RemoveSession(SessionID);
            SetCurrentSession("");
        }

        /// <summary>
        /// Checks whether the user is currently logged in by checking sessionID.
        /// </summary>
        /// <returns>If the user is logged in, returns true, false otherwise.</returns>
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

        private static bool CheckIfSessionExists(string sessionID)
        {
            SessionId sessionId = new(sessionID);
            try
            {
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
