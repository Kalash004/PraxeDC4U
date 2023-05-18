using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Models;

namespace SessionService.SessionTemplate_Creater
{
	public class SessionIdCreaterSingleton
	{
		private static SessionIdCreaterSingleton? instance = null;
		private SessionIdCreaterSingleton()
		{
		}

		public static SessionIdCreaterSingleton Instanciate()
		{
			if (instance == null)
			{
				instance = new SessionIdCreaterSingleton();
			}
			return instance;
		}
		public SessionId CreateSessionId(DBUser user) 
		{
			string datetime = DateTime.Now.ToString();
			string toBeHashedValue = user.Name + datetime;
			byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(toBeHashedValue);
			byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
			return new SessionId(ByteArrayToString(tmpHash));
		}
		private string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
