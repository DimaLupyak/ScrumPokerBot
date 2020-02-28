using Firebase.Database;
using System;
using System.Threading.Tasks;

namespace BotsController.DAL
{
    public class Repository
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

        public async Task AddAsync(string data)
        {
            await firebase.Child("test").PostAsync(data);
        }

        public async Task<string> GetDataAsync()
        {
            var dinos = await firebase.Child("test").OnceAsync<string>();

            foreach (var dino in dinos)
            {
                return dino.ToString()
            }
            return "";
        }
    }
}
