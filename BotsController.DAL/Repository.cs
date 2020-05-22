using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotsController.DAL
{
    public class Repository<T>
    {
        protected readonly FirebaseClient firebase;
        public Repository(string authSecret, string url)
        {
            firebase = new FirebaseClient(
              url,
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(authSecret)
              });
        }

        public async Task AddAsync(T resource)
        {
            var user = await firebase
              .Child(typeof(T).Name)
              .PostAsync(JsonConvert.SerializeObject(resource));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var resources = await firebase
               .Child(typeof(T).Name)
               .OnceAsync<T>();

            return resources.Select(x => x.Object);
        }
    }

    
}
