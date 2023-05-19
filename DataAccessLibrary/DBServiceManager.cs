using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DAOS;
using DataTemplateLibrary.Models;

namespace DataAccessLibrary
{
    public class DBServiceManager
    {
        ServiceDAO serviceDAO = new ServiceDAO();

        public ReturnData<DBService?,string> CreateService(DBService service)
        {
            int returnedID = serviceDAO.Create(service);
            service.ID = returnedID;
            return new ReturnData<DBService?, string>(service,"Created");
        }
        public List<DBService?> GetAllServiceByUser(DBUser user)
        {
            // TODO : Check for null and throw exception if null
            return serviceDAO.GetAllByUserID(user.ID);
        }

        public DBService? GetOneServiceByUserAndId(DBUser user, int id)
        {
            foreach (var service in GetAllServiceByUser(user))
            {
                if (service.ID == id) return service;
            }
            return null;
        } 

    }
}
