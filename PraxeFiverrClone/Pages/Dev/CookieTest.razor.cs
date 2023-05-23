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

            CookieManager.SetSessionCookie("COOKIE_SESSION_STRING", "I CAN SEE YOU");
            string sessionCookieString = await CookieManager.GetSessionCookieString("Cookie");
            Console.WriteLine("[DEV_COOKIES] COOKIE_SESSION_STRING: " + sessionCookieString);

            CookieManager.SetCookie("COOKIE_STRING", "THE ANSWER TO THE ULTIMATE QUAESTION OF LIFE, THE UNIVERSE AND EVERYTHING IS");
            CookieManager.SetCookie("COOKIE_INT", 42);
            CookieManager.SetCookie("COOKIE_FLOAT", 0.0f);

            string cookieString = await CookieManager.GetCookieString("COOKIE_STRING");
            int cookieInt = await CookieManager.GetCookieInt("COOKIE_INT");
            float cookieFloat = await CookieManager.GetCookieFloat("COOKIE_FLOAT");

            Console.WriteLine("[DEV_COOKIES] COOKIE_STRING: " + cookieString);
            Console.WriteLine("[DEV_COOKIES] COOKIE_INT: " + cookieInt);
            Console.WriteLine("[DEV_COOKIES] COOKIE_FLOAT: " + cookieFloat);
        }
    }
}
