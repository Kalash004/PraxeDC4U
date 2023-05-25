using DataTemplateLibrary.Models;
using Microsoft.AspNetCore.Components;
using LoginService;

namespace PraxeFiverrClone.Pages.Dev
{
    public partial class LoginTestPage : ComponentBase
    {
        [Inject]
        public LoginManager? LoginManager { get; set; } = null;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if(LoginManager != null)
                {
                    await LoginManager.Fetch();
                    StateHasChanged();
                }       
            }
        }

        public void Login()
        {
            if(LoginManager == null)
            {
                return;
            }

            DBUser user = new("Admin", "Admin");
            try{
                LoginManager.Login(user);
            }
            catch(LoginSignupException e)
            {
                Console.WriteLine(e.Message);
            }
            
            StateHasChanged();
        }
        public void Logout()
        {
            LoginManager?.Logout();
        }
    }
}
