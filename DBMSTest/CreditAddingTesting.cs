using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Models;
using ServerManagement;
using SessionService.SessionTemplate_Creater;

namespace Tests
{
    [TestClass]
    public class CreditAddingTesting
    {
        [TestMethod]
        public void TestingAddingCredits()
        {
            // Arrange
            ServerManager manager = new ServerManager();
            Random random = new Random();
            DBUser user = new DBUser($"{random.Next().ToString()}:TestingAddingCredits",$"{random.Next().ToString()}",0);
            // Act
            user = manager.SingUpUser(user);
            SessionId sessionId = manager.LogUserInCreateSession(user).Result;
            DBUser updatedUser = manager.BuyCredits(sessionId, 100);
            // Assert
            Assert.AreEqual(100,updatedUser.CurrentCredits);
        }
    }
}
