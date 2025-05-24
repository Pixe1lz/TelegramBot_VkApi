using Telegram.Bot;

namespace First_TelegramBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token_bot = new TelegramBotClient("7626928336:AAF508V9IVJJ9ZrYxM9oIkF7c4ghqjna_Ns");

            var MetBot = new BotEngine(token_bot);

            await MetBot.ListenForMessagesAsync();
        }
    }
}
