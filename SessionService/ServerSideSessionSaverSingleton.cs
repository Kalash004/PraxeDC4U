using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Models;
using SessionService.SessionTemplate_Creater;

namespace SessionService
{
	public class ServerSideSessionSaverService
	{
		private static ServerSideSessionSaverService instance = null;
		private Dictionary<SessionId, DBUser> currentSessions = new Dictionary<SessionId, DBUser>();
		private ServerSideSessionSaverService()
		{
		}

		public static ServerSideSessionSaverService GetInstance()
		{
			if (instance == null)
			{
				instance = new ServerSideSessionSaverService();
			} 
			return instance;
		}
		public void AddSession(SessionId sessionId,DBUser user)
		{
			currentSessions.Add(sessionId, user);
		}

		public DBUser GetUserFromSessionId(SessionId id)
		{
			if (SessionExists(id))
			{
				return currentSessions[id];
			}
			else throw new Exception($"Session id: \"{id}\" doesnt exist");
		}

		public bool SessionExists(SessionId id)
		{
			return currentSessions.ContainsKey(id);
		}

		public SessionId AddSession(DBUser user)
		{
			var sessionId = SessionIdCreaterSingleton.Instanciate().CreateSessionId(user);
			AddSession(sessionId, user);
			return sessionId;
		}
	}
}
