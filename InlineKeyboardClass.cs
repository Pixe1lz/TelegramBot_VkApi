using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace First_TelegramBot
{
    class InlineKeyboardClass
    {
        private readonly TelegramBotClient _statusBot;

        public InlineKeyboardClass (TelegramBotClient statusBot)
        {
            _statusBot = statusBot;
        }

        public async Task LineMarkup(long sender_chatId)
        {
            var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
            {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Обычное расписание", "callback_button_regularSchedule")
                    },
                    new []
                    {
                         InlineKeyboardButton.WithCallbackData("Изменения в расписании", "callback_button_newSchedule")
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Auto сообщения", "callback_button_autoMessage")
                    }
            });

            await _statusBot.SendMessage(
                chatId: sender_chatId,
                text: "Что бы вы хотели узнать?",
                replyMarkup: keyboard
            );
        }

        public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            string callbackData = callbackQuery.Data;

            switch (callbackData)
            {
                case "callback_button_regularSchedule":
                    await botClient.DeleteMessage(
                        chatId: callbackQuery.Message.Chat.Id,
                        messageId: callbackQuery.Message.MessageId,
                        cancellationToken: cancellationToken
                    );

                    await botClient.SendMessage(
                        chatId: callbackQuery.Message.Chat.Id,
                        text: """
                        Конечно, вот текущее расписание: 
                        - Понедельник:
                            - Русский
                            - Математика
                            - Инглиш
                        - Вторник:
                            - География
                            - Биология
                            - Физкультура       
                        """,
                        cancellationToken: cancellationToken
                    );

                    break;

                case "callback_button_newSchedule":
                    await botClient.DeleteMessage(
                        chatId: callbackQuery.Message.Chat.Id,
                        messageId: callbackQuery.Message.MessageId,
                        cancellationToken: cancellationToken
                    );

                    ExtractImageClass EIC = new ExtractImageClass("vk1.a.q29iieDDTtUF51ky6qhIk0NvLCj1PNUKNebOGFV8aJE1VDPzxg0ufBJDQlTUv82UDiBlDcXUbFk2W8Fk2d7SvoJbxYuICTX04pfVRY9bbPa_01hR1DIUk17U6fs7LBIarVYVxmqnWQQQHQirFaFk6MBaac8Vxw1s5gd3VUclIJ0AKLCuFLrNz7UimLAJupikJpps6q1rsLL0r9S_Zr55Pg", "230256361");
                    await EIC.Run();

                    List<string> photosList = EIC.SendGetterPhotoListURL();

                    foreach (var photo in photosList)
                    {
                        await botClient.SendPhoto(
                        chatId: callbackQuery.Message.Chat.Id,
                        photo: photo,
                        cancellationToken: cancellationToken
                        );
                    }

                    LineMarkup(callbackQuery.Message.Chat.Id);

                    break;

                case "callback_button_autoMessage":
                    BotEngine bot = new BotEngine(_statusBot);

                    break;
            }

            await botClient.AnswerCallbackQuery(
                callbackQueryId: callbackQuery.Id,
                text: "Вы выбрали: " + callbackData,
                cancellationToken: cancellationToken
            );
        }
    }
}
