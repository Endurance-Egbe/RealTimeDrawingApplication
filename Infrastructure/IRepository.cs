using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace View.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void CreateModel(T model);
        T GetModel(string value);
        void UpdateModel(T Model);
        void DeleteModel(T Model);

        IEnumerable<T> Find(Expression<Func<T, bool>> Predicate);

    }
}
