    using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace View.Infrastructure
{
    public class EFCoreRepository<T> : IRepository<T> where T : class
    {
        public UserDbContext userContext;
        public EFCoreRepository(UserDbContext sqlType)
        {
            userContext = sqlType;
        }
        public void CreateModel(T model)
        {
            userContext.Add(model);
            userContext.SaveChanges();
        }

        public void DeleteModel(T model)
        {

            userContext.Remove(model);
            userContext.SaveChanges();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> Predicate)
        {
            return userContext.Set<T>().Where(Predicate);
        }

        public T GetModel(string value)
        {

            T model = userContext.Find<T>(value);
            return model;
        }

        public void UpdateModel(T model)
        {
            userContext.Update(model);
            userContext.SaveChanges();
        }


    }
}
