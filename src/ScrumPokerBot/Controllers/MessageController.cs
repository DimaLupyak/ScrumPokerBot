using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerBot.Models;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Controllers
{
    [Route("api/message/update")]
    public class MessageController : Controller
    {
        // POST api/values
        [HttpPost]
        public async Task<OkResult> Post([FromBody]Update update)
        {
            if (update == null) return Ok();

            var commands = Bot.Commands;
            var message = update.Message;
            var botClient = await Bot.GetBotClientAsync();

            foreach (var command in commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, botClient);
                    break;
                }
            }
            return Ok();
        }
    }
}