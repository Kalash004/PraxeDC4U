using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Interfaces
{
    public interface IDAO<T> where T : IBaseClass
    {
        T? GetByID(int id);
        List<T> GetAll();
        int Create(T element);
        void Save(T element);
        void Delete(int id);
    }
}
