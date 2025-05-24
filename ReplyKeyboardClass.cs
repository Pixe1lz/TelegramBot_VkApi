using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace First_TelegramBot
{
    class ReplyKeyboardClass
    {
        private readonly TelegramBotClient _statusBot;

        public ReplyKeyboardClass(TelegramBotClient statusBot)
        {
            _statusBot = statusBot;
        }

        public async Task LineMarkup(long sender_id)
        {
            var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("")
                    }
                },

                ResizeKeyboard = false, // Чтоб клавиатура была адаптивной
                OneTimeKeyboard = true // Клавиатура на одно нажатие
            };
        }
    }
}
