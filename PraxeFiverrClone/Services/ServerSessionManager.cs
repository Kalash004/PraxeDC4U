using System.Security.Cryptography;
using System.Text;

namespace SessionService
{
	/// <summary>
	/// Singleton service that works with session ids.
	/// Cant be connected to blazor directly
	/// </summary>
	/// <creator>Anton Kalashnikov</creator>
	/// <edited>Ondřej Kačírek</edited>
	public class ServerSessionManager
	{
		private static ServerSessionManager? instance;
		public static ServerSessionManager Instance { 
			get 
			{ 
				instance ??= new ServerSessionManager();
				return instance; 
			} 
		}

		private readonly Dictionary<string, int> currentSessions = new();
		private ServerSessionManager()
		{
		}

		public string GenerateNewSession(int userId)
		{
			string sessionId = GenerateSessionId(userId);
            AddSession(sessionId, userId);
			return sessionId;
		}
		
        private void AddSession(string sessionId, int userId)
        {
            currentSessions.Add(sessionId, userId);
        }
   
        public void RemoveSession(string sessionId)
		{
			currentSessions.Remove(sessionId);

        }
		
		public int GetUserIdFromSessionId(string sessionId)
		{
			if (SessionExists(sessionId))
			{
				return currentSessions[sessionId];
			}
			else throw new Exception($"Session id: \"{sessionId}\" doesnt exist");
		}

		public bool SessionExists(string sessionId)
		{
			return currentSessions.ContainsKey(sessionId);
		}

        public static string GenerateSessionId(int userId)
        {
            string datetime = DateTime.Now.ToString();
            string toBeHashedValue = userId + datetime;
            byte[] tmpSource = Encoding.ASCII.GetBytes(toBeHashedValue);
            byte[] tmpHash = MD5.HashData(tmpSource);
            return ByteArrayToString(tmpHash);
        }
        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
