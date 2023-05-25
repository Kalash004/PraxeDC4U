using CookieService;
using DataTemplateLibrary.Models;
using MySqlX.XDevAPI.Common;
using ServerManagement;
using SessionService;
using System.Security.Cryptography;
using System.Text;

namespace LoginService
{
    public class LoginManager
    {
        protected CookieManager? CookieManager { get; set; }
        protected ServerManager? ServerManager { get; set; }

        /// <summary>
        /// This event is invoked when user is logged in.
        /// This includes the first moment the user enters the website.
        /// </summary>
        public event Action? OnLogin;
        /// <summary>
        /// This event is being invoked whenever user logs out.
        /// </summary>
        public event Action? OnLogout;
        /// <summary>
        /// This event is being invoked whenever user logs in.
        /// </summary>
        public event Action? OnUpdate;

        public string SessionID { get; private set; } = "";
        public int UserID { get => GetUserID(); }
        public bool LoggedIn { get => IsLoggedIn(); }

        public LoginManager(CookieManager cookieManager, ServerManager serverManager)
        {
            CookieManager = cookieManager;
            ServerManager = serverManager;
        }

        /// <summary>
        /// Fetches all the data required.
        /// </summary>
        public async Task Fetch()
        {
            if (CookieManager != null)
            {
                string sessionID = await CookieManager.GetSessionCookieString("sessionID");
                if (CheckIfSessionExists(sessionID))
                {
                    SessionID = sessionID;
                    OnLogin?.Invoke();
                }
                else
                {
                    SetCurrentSession("");
                }
            }
            OnUpdate?.Invoke();
            return;
        }

        /// <summary>
        /// Logs in the user usig it's credentials.
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="LoginSignupException"></exception>
        public void Login(string username, string password)
        {
            if (IsLoggedIn())
            {
                Logout();
            }
            if (ServerManager == null)
            {
                throw new LoginSignupException("The ServerManager is null");
            }

            try
            {
                DBUser user = new(username, HashString(password + "DC4U"));
                Console.WriteLine(user.HashedPassword);
                string sessionID = ServerManager.LogUserInCreateSession(user);
                SetCurrentSession(sessionID);
            }
            catch(Exception e)
            {
                throw new LoginSignupException(e.Message);
            }
            OnLogin?.Invoke();
            OnUpdate?.Invoke();
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        public void Logout()
        {
            ServerSessionManager.Instance.RemoveSession(SessionID);
            SetCurrentSession("");
            OnLogout?.Invoke();
            OnUpdate?.Invoke();
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

        private static string HashString(string value)
        {
            byte[] data = Encoding.ASCII.GetBytes(value);
            byte[] hashedData = SHA256.Create().ComputeHash(data);
            string hash = BitConverter
               .ToString(hashedData)
               .Replace("-","");
            return hash;
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