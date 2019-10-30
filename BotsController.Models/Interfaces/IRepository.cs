using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BotsController.Models.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        string Insert(T item);
        void Update(T item);
        void Delete(string id);
    }
}