using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace First_TelegramBot
{
    class BotEngine
    {
        private readonly TelegramBotClient _statusBot;
        private long chatId; // Айди чата для проверки постов

        public BotEngine(TelegramBotClient statusBot)
        {
            _statusBot = statusBot;
        }

        public async Task ListenForMessagesAsync()
        {
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _statusBot.StartReceiving (
                updateHandler: HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var client = await _statusBot.GetMe();

            Console.WriteLine($"Начали прослушку в боте: @{client.Username}");    

            Console.ReadLine();
        }

        // Обработчик обновлений, отслеживает сообщения
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            InlineKeyboardClass inline_keyboard_Class = new InlineKeyboardClass(_statusBot);
            // ReplyKeyboardClass reply_keyboard_Class = new ReplyKeyboardClass(_statusBot);

            if (update.Message is { } message)
            {
                if (message.Text is { } messageText)
                {
                    Console.WriteLine($"{message.Chat.FirstName} отправил сообщение боту. ('{messageText}')");

                    if (messageText.Equals("/start"))
                    {
                        chatId = update.Message.Chat.Id;
                        await inline_keyboard_Class.LineMarkup(message.Chat.Id); // inline-keyboard
                        _ = StartCheckNewPosts();
                    }
                    // await reply_keyboard_Class.LineMarkup(message.Chat.Id); // reply-keyboard
                }
            } else if (update.CallbackQuery is { } callbackQuery) {
                await inline_keyboard_Class.HandleCallbackQueryAsync(botClient, callbackQuery, cancellationToken);
            }  
        }

        // обработчик ошибок
        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Polling error: {exception.Message}");
            return Task.CompletedTask;
        }

        public async Task StartCheckNewPosts()
        {
            ExtractImageClass EIC = new ExtractImageClass("vk1.a.q29iieDDTtUF51ky6qhIk0NvLCj1PNUKNebOGFV8aJE1VDPzxg0ufBJDQlTUv82UDiBlDcXUbFk2W8Fk2d7SvoJbxYuICTX04pfVRY9bbPa_01hR1DIUk17U6fs7LBIarVYVxmqnWQQQHQirFaFk6MBaac8Vxw1s5gd3VUclIJ0AKLCuFLrNz7UimLAJupikJpps6q1rsLL0r9S_Zr55Pg", "230256361");

            while (true)
            {
                await EIC.Run();
                List<string> photoUrl = EIC.SendGetterPhotoListURL();

                foreach (var photo in photoUrl)
                {
                    await _statusBot.SendPhoto(
                    chatId: chatId,
                    photo: photo
                    );
                }

                photoUrl.Clear();

                await Task.Delay(TimeSpan.FromMinutes(0.1));
            }
        }
    }
}