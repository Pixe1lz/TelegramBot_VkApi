using Telegram.Bot;

namespace First_TelegramBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token_bot = new TelegramBotClient("###");

            var MetBot = new BotEngine(token_bot);

            await MetBot.ListenForMessagesAsync();
        }
    }
}
