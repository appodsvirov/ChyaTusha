using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ChyaTusha
{
    public class UpdateHandler
    {
        private Dictionary<long, string> _userStates;
        private Sender _sender;
        public UpdateHandler(Dictionary<long, string> userStates, ITelegramBotClient botClient)
        {
            _userStates = userStates;
            _sender = new(botClient);
        }

        //Start 
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message) return;

            long chatId = message.Chat.Id;
            string userState = _userStates.ContainsKey(chatId) ? _userStates[chatId] : "start";

            if (message.Text != null)
            {
                if (message.Text.ToLower() == "/start")
                {
                    // Начать квест
                    await StartGame(botClient, chatId);
                }
                else
                {
                    switch (userState)
                    {
                        case "StartGame":
                            await HandleStage1(botClient, chatId, message.Text);
                            break;
                        case "Fork":
                            await HandleStage2(botClient, chatId, message.Text);
                            break;
                        default:
                            await botClient.SendMessage(chatId, "Я не понял ваш выбор. Напишите /start, чтобы начать.");
                            break;
                    }
                }
            }
        }


        async Task StartGame(ITelegramBotClient botClient, long chatId)
        {
            _userStates[chatId] = "StartGame";
            var replyMarkup = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton("Да начнется игра!😈")
                })
            {
                ResizeKeyboard = true
            };

            await _sender.TrySendPhoto(chatId, "StartGame.png", replyMarkup);
        }

        async Task HandleStage1(ITelegramBotClient botClient, long chatId, string messageText)
        {
            _userStates[chatId] = "Fork";
            var replyMarkup = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton("Лес"),
                    new KeyboardButton("Пещера"),
                    new KeyboardButton("Водопад"),
                })
            {
                ResizeKeyboard = true
            };

            await _sender.TrySendPhoto(chatId,
                "forkInTheRoad.png",
                replyMarkup,
                "В темные времена, когда магия и реальность переплетаются," +
                " существовала одна загадка, которая не давала покоя самым лучшим детективам. " +
                "Это история об убийстве, которое никто не смог раскрыть — о таинственной туше и том, кто её убил." +
                " Легенда гласит, что разгадка этой тайны скрыта в трех путях, каждый из которых ведет к ключевым уликам: " +
                "«Лес», «Водопад» и «Пещера». Но будьте осторожны: путь к истине нелегок," +
                " а самой главной угрозой является не только убийца, но и те, кто пытаются скрыть правду.");

        }

        async Task HandleStage2(ITelegramBotClient botClient, long chatId, string messageText)
        {
            if (messageText == "Лес")
            {
                _userStates[chatId] = "stage3";
                await botClient.SendMessage(chatId, "Вы победили дракона и вышли из леса! Поздравляю!");
            }
            else if (messageText == "Пещера")
            {
                _userStates[chatId] = "stage3";
                await botClient.SendMessage(chatId, "Вы использовали артефакт и нашли выход из леса! Поздравляю!");
            }
            else if (messageText == "Водопад")
            {
                await botClient.SendMessage(chatId, "Пожалуйста, выберите: 'победить дракона' или 'использовать артефакт'.");
            }
        }
    }
}
