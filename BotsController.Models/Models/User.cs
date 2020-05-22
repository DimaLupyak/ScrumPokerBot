using System;
using Newtonsoft.Json;

namespace BotsController.Core.Models
{
    public class User
    {
        [JsonProperty]
        public string UserId { get; set; }
        [JsonProperty]
        public string ChatId { get; set; }
        [JsonProperty]
        public string UserName { get; set; }
        [JsonProperty]
        public DateTime DateTime { get; set; }
    }

}
