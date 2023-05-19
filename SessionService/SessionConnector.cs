using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Models;
using SessionService.SessionTemplate_Creater;

namespace SessionService
{
    public class SessionConnector
    {
        ServerSideSessionSaverService sessionManager = ServerSideSessionSaverService.GetInstance();

        public SessionId AddSession(DBUser user)
        {
            return sessionManager.AddSession(user);
        }
    }
}
