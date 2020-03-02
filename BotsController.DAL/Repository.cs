using Firebase.Database;
using Newtonsoft.Json;
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

        public async Task AddPidarAsync(string userName)
        {
            var pidar = new Pidar()
            {
              //  DateTime = DateTime.Now,
                UserName = userName
            };
            var dino = await firebase
              .Child("pidars")
              .PostAsync(JsonConvert.SerializeObject(pidar));
        }

        public async Task<string> GetPidarAsync()
        {
            var pidars = await firebase
               .Child("pidars")
               .OnceAsync<Pidar>();

            foreach (var pidar in pidars)
            {
                return $"{pidar.Object.UserName}";
            }
            return "";
        }
    }

    public class Pidar
    {
        [JsonProperty]
        public string UserName { get; set; }
       // [JsonProperty]
       // public DateTime DateTime { get; set; }
    }
}
