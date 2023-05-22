using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DAOS;
using DataTemplateLibrary.Models;

namespace DataAccessLibrary.DBChildManagers
{
    public class DBTransactionManager
    {
        private TransactionDAO transactionDAO = new TransactionDAO();
        // create transaction
        public DBTransaction? CreateTransaction(DBTransaction transaction)
        {
            int id = transactionDAO.Create(transaction);
            transaction.ID = id;
            return transaction;
        }
        // read transactions by user
        public List<DBTransaction> ReadTransactionsByUserId(int id)
        {
            // check for null
            List<DBTransaction> transaction = transactionDAO.GetAllByUserId(id);
            return transaction;
        }
        // read transactions by service_id
        public List<DBTransaction> ReadTransactionsByServiceId(int id)
        {
            return transactionDAO.GetAllByServiceId(id);
        }

    }
}
