using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataTemplateLibrary.Models;
using SessionService.SessionTemplate_Creater;

namespace Tests
{
    [TestClass]
    public class DBServiceReadTest
    {
        [TestMethod]
        public void ReadingServicesWithSessionID()
        {
            // Arrange
            DBManager manager = new DBManager();
            DBUser user = new DBUser("1455980865", "565440666");
            var data = manager.LogUserInCreateSession(user);
            SessionId sessionId = data.Result;
            // Act
            List<DBService?> services = manager.GetAllUserServices(sessionId);
            DBService concrete_service = manager.GetServiceFromDB(sessionId, services.First().ID);
            // Assert
            Assert.IsNotNull(services);
            Assert.IsTrue(services.Any());
            Assert.AreEqual(concrete_service.ServiceName, services.First().ServiceName);
            
        } 
    }
}
