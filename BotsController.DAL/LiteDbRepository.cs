using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BotsController.Core.Interfaces;
using BotsController.Models.Data;
using LiteDB;

namespace BotsController.DAL
{
    public class LiteDbRepository<T> : IRepository<T> where T : IIdentifiable
    {
        public LiteCollection<T> Collection { get; private set; }

        public LiteDbRepository(LiteDbContext context)
        {
            Collection = context.SetCollection<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return Collection.FindAll();
        }

        public T Get(string id)
        {
            return Collection.FindOne(s => s.Id == id);
        }

        public string Insert(T item)
        {
            return Collection.Insert(item);
        }

        public void Update(T item)
        {
            if (!Collection.Update(item))
            {
                throw new KeyNotFoundException($"Element with id '{item.Id}' is not found.");
            }
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Collection.Find(predicate);
        }

        public void Delete(string id)
        {
            Collection.Delete(x => x.Id == id);
        }
    }
}