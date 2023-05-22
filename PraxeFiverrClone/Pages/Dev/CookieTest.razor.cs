using Microsoft.AspNetCore.Components;
using CookieService;

namespace PraxeFiverrClone.Pages.Dev
{
    public partial class CookieTestPage : ComponentBase
    {
        [Inject]
        private CookieManager? CookieManager { get; set; } = null;
        protected override async void OnInitialized()
        {
            if (CookieManager == null)
            {
                return;
            }

            CookieManager.SetSessionCookie("Cookie", "Wololo");
            string sessionCookieString = await CookieManager.GetSessionCookieString("Cookie");
            Console.WriteLine(sessionCookieString);

            CookieManager.SetCookie("COOKIE_STRING", "Marx");
            CookieManager.SetCookie("COOKIE_INT", 120);
            CookieManager.SetCookie("COOKIE_FLOAT", 15.5f);

            string cookieString = await CookieManager.GetCookieString("COOKIE_STRING");
            Console.WriteLine(cookieString);

            int cookieInt = await CookieManager.GetCookieInt("COOKIE_INT");
            Console.WriteLine(cookieInt);

            float cookieFloat = await CookieManager.GetCookieFloat("COOKIE_FLOAT");
            Console.WriteLine(cookieFloat);
        }
    }
}
