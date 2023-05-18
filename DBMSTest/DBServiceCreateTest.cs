using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataTemplateLibrary.Models;

namespace Tests
{
    [TestClass]
    public class DBServiceCreateTest
    {
        [TestMethod]
        public void CreationTesting()
        {
            // Arrange
            DBUserManager u_manager = new DBUserManager();
            DBServiceManager s_manager = new DBServiceManager();
            Random rand = new Random();
            string name = rand.Next().ToString();
            string pass = rand.Next().ToString();
            DBUser user = new DBUser(name,pass);
            user = u_manager.SingUpUser(user).Result;
            DBService service = new DBService(user.ID,"test",0,DateOnly.Parse("2023-05-18"),null,true,"testingtesting",null,null);
            // Act
            DBService service_from_db = s_manager.CreateService(service).Result;
            DBService service_from_db_after_creation = s_manager.GetOneServiceByUserAndId(user, service_from_db.ID);
            // Assert
            Assert.AreEqual(service_from_db,service_from_db_after_creation);
        }
    }
}
