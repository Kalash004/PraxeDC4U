using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataTemplateLibrary;
using DataTemplateLibrary.Models;
using SessionService;
using ServerManagement;

namespace Tests
{
    /// <summary>
    /// Test for session creation.
    /// Also test if admin exists in the database
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    [TestClass]
    public class SessionCreaterTest
    {
        [TestMethod]
        public void TestSession()
        {
            // Arrange
            DBUser user = new DBUser("Admin", "Admin");
            ServerManager manager = new ServerManager();
            // Act
            var data_from_db = manager.LogUserInCreateSession(user);
            var user_from_runtime_id = ServerSideSessionSaverService.GetInstance().GetUserFromSessionId(data_from_db.Result);
            // Assert
            Assert.AreEqual(user_from_runtime_id, data_from_db.Message.ID);
        }
    }
}
