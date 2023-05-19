using DataTemplateLibrary.Models;
using DataTemplateLibrary;
using DataAccessLibrary.DBChildManagers;

namespace Tests
{
    [TestClass]
    public class DBUserCreateTest
    {
        [TestMethod]
        public void CreationTesting()
        {
            // Arrange
            Random rand = new Random();
            string name = rand.Next().ToString();
            string pass = rand.Next().ToString();
            DBUser user = new DBUser(name,pass,0);
            DBUserManager manager = new DBUserManager();
            // Act
            manager.SingUpUser(user);
            DBUser taken_user = manager.GetUserByName(user.Name);
            manager.RemoveUser(user);
            // Assert
            Assert.AreEqual(user.Name,taken_user.Name);
            Assert.AreEqual(user.ID, taken_user.ID);
        }
    }
}