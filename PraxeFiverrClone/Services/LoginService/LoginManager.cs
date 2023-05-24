using CookieService;
using DataTemplateLibrary.Models;
using ServerManagement;
using SessionService;

namespace LoginService
{
    public class LoginManager
    {
        protected CookieManager? CookieManager { get; set; }
        protected ServerManager? ServerManager { get; set; }

        public string SessionID { get; private set; } = "";
        public int UserID { get => GetUserID(); }
        public bool LoggedIn { get => IsLoggedIn(); }

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
        /// Logs in the user usig it's credentials.
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="LoginSignupException"></exception>
        public void Login(DBUser user)
        {
            if (IsLoggedIn())
            {
                throw new LoginSignupException("The user is already logged in");
            }
            if (ServerManager == null)
            {
                throw new LoginSignupException("The ServerManager is null");
            }

            try
            {
                string sessionID = ServerManager.LogUserInCreateSession(user);
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
            ServerSessionManager.Instance.RemoveSession(SessionID);
            SetCurrentSession("");
        }

        private int GetUserID()
        {
            try
            {
                return ServerSessionManager.Instance.GetUserIdFromSessionId(SessionID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }
        private bool IsLoggedIn()
        {
            bool loginStatus = false;
            if (CookieManager != null)
            {
                if (CheckIfSessionExists(SessionID))
                {
                    loginStatus = true;
                }
            }
            return loginStatus;
        }

        private static bool CheckIfSessionExists(string sessionId)
        {
            try
            {
                if (ServerSessionManager.Instance.SessionExists(sessionId))
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
