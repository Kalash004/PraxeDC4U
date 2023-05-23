﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataTemplateLibrary.Models;
using SessionService.SessionTemplate_Creater;
using ServerManagement;

namespace Tests
{
    /// <summary>
    /// Test for reading service obejcts form database
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    [TestClass]
    public class DBServiceReadTest
    {
        [TestMethod]
        public void ReadingServicesWithSessionID()
        {
            // Arrange
            ServerManager manager = new();
            DBUser user = new DBUser("1455980865", "565440666");
            ReturnData<SessionId, DBUser> data;
            try
            {
                data = manager.LogUserInCreateSession(user);
            }
            catch (NullReferenceException e)
            {
                user = manager.SingUpUser(user);
                data = manager.LogUserInCreateSession(user);
            }
            SessionId sessionId = data.Result;
            // Act
            List<DBService?> services;
            DBService concrete_service;
            try
            {
                services = manager.GetAllUserServices(sessionId);
                concrete_service = manager.GetServiceFromDB(sessionId, services.First().ID);
            } catch (System.InvalidOperationException e)
            {
                manager.CreateService(sessionId, new DBService(user.ID, "DBServiceReadTest", -1, DateOnly.FromDateTime(DateTime.Now), null, true, "DBServiceReadTest", "DBServiceReadTest", null, false));
                services = manager.GetAllUserServices(sessionId);
                concrete_service = manager.GetServiceFromDB(sessionId, services.First().ID);
            }
            // Assert
            Assert.IsNotNull(services);
            Assert.IsTrue(services.Any());
            Assert.AreEqual(concrete_service.ServiceName, services.First().ServiceName);

        }
    }
}
