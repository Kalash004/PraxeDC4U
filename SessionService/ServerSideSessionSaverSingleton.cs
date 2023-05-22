using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Models;
using SessionService.SessionTemplate_Creater;

namespace SessionService
{
	/// <summary>
	/// Singleton service that works with session ids.
	/// Cant be connected to blazor directly
	/// </summary>
	/// <creator>Anton Kalashnikov</creator>
	public class ServerSideSessionSaverService
	{
		private static ServerSideSessionSaverService instance = null;
		private Dictionary<string, int> currentSessions = new Dictionary<string, int>();
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
		public void AddSession(SessionId sessionId,int userId)
		{
			currentSessions.Add(sessionId.Id, userId);
		}

		public int GetUserFromSessionId(SessionId id)
		{
			if (SessionExists(id))
			{
				return currentSessions[id.Id];
			}
			else throw new Exception($"Session id: \"{id}\" doesnt exist");
		}

		public bool SessionExists(SessionId id)
		{
			return currentSessions.ContainsKey(id.Id);
		}

		public SessionId AddSession(DBUser user,int userId)
		{
			var sessionId = SessionIdCreaterSingleton.Instanciate().CreateSessionId(user);
			AddSession(sessionId, userId);
			return sessionId;
		}
	}
}
