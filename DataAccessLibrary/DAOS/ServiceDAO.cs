using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Interfaces;
using DataTemplateLibrary.Models;
using MySql.Data.MySqlClient;

namespace DataAccessLibrary.DAOS
{
    public class ServiceDAO : AbstractDAO<DBService>
    {
        protected override DBService Map(MySqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected override List<MySqlParameter> Map(DBService obj)
        {
            throw new NotImplementedException();
        }
    }
}
