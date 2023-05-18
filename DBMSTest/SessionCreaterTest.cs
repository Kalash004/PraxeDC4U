using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataTemplateLibrary;
using DataTemplateLibrary.Models;
using SessionService;

namespace Tests
{
    [TestClass]
    public class SessionCreaterTest
    {
        [TestMethod]
        public void TestSession()
        {
            // Arrange
            DBUser user = new DBUser("Admin", "Admin");
            DBManager manager = new DBManager();
            // Act
            var data_from_db = manager.LogUserInCreateSession(user);
            var user_from_runtime = ServerSideSessionSaverService.GetInstance().GetSession(data_from_db.Result);
            // Assert
            Assert.AreEqual(user_from_runtime, data_from_db.Message);
        }
    }
}
