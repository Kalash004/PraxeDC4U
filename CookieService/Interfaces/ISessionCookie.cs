namespace CookieService
{
    public interface ISessionCookie
    {
        Task SetValue(string key, string value);
        Task<string> GetValue(string key, string def = "");
    }
}
