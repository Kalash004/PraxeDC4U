using DataTemplateLibrary.Models;

namespace PraxeFiverrClone
{
    public class MyIdService
    {
        public SessionService.SessionTemplate_Creater.SessionId id { get; set; } = new SessionService.SessionTemplate_Creater.SessionId("");
        public DBUser User { get; set; } = new DBUser("", "");
    }
}
