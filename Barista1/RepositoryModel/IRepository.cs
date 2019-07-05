using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista1
{
    interface IRepository<T>
        where T : class
    {
        SqlCommand GetList(); // получение всех объектов
        void Add(params object[] list); // добавление объекта
    }
}
