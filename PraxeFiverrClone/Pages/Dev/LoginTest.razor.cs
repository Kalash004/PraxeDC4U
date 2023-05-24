using DataTemplateLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace PraxeFiverrClone.Pages.Dev
{
    public partial class LoginTestPage : ComponentBase
    {
        [Inject]
        public UserContext? UserContext { get; set; } = null;
        public bool IsLoggedIn { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                if (UserContext == null)
                {
                    return;
                }

                IsLoggedIn = await UserContext.IsLoggedIn();
                StateHasChanged();
            }
        }

        public async void Login()
        {
            if(UserContext == null)
            {
                return;
            }

            DBUser user = new("Admin", "Admin");
            await UserContext.Login(user);
            IsLoggedIn = true;
            StateHasChanged();
        }
        public void Logout()
        {
            UserContext?.Logout();
            IsLoggedIn = false;
        }
    }
}
