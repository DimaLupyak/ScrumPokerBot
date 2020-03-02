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

        public async Task AddAsync(string data)
        {
            var dino = await firebase
              .Child("dinosaurs")
              .PostAsync(JsonConvert.SerializeObject(new Dinosaur()));
        }

        public async Task<string> GetDataAsync()
        {
            var dinos = await firebase
               .Child("dinosaurs")
               .OnceAsync<Dinosaur>();

            foreach (var dino in dinos)
            {
                return dino.ToString();
            }
            return "";
        }
    }

    public class Dinosaur
    {
        [JsonProperty()]
        public double H { get; set; }
    }
}
