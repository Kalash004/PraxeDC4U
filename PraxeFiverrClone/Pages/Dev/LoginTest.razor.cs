using DataTemplateLibrary.Models;
using Microsoft.AspNetCore.Components;
using LoginService;

namespace PraxeFiverrClone.Pages.Dev
{
    public partial class LoginTestPage : ComponentBase
    {
        [Inject]
        public LoginManager? LoginManager { get; set; } = null;
        public bool IsLoggedIn { get; set; }
        public string SessionID { get; set; } = "";
        public int UserID { get; set; } = -1;

        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                if (LoginManager == null)
                {
                    return;
                }

                SessionID = LoginManager.SessionID;
                UserID = LoginManager.GetUserID();
                StateHasChanged();
            }
        }

        public async void Login()
        {
            if(LoginManager == null)
            {
                return;
            }

            DBUser user = new("Admin", "Admin");
            try{
                await LoginManager.Login(user);
            }
            catch(LoginSignupException e)
            {
                Console.WriteLine(e.Message);
            }
            
            IsLoggedIn = true;
            StateHasChanged();
        }
        public void Logout()
        {
            LoginManager?.Logout();
            IsLoggedIn = false;
        }
    }
}
