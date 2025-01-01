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
                    _userStates[chatId] = "stage1";
                    await SendStage1(botClient, chatId);
                }
                else
                {
                    switch (userState)
                    {
                        case "stage1":
                            await HandleStage1(botClient, chatId, message.Text);
                            break;
                        case "stage2":
                            await HandleStage2(botClient, chatId, message.Text);
                            break;
                        default:
                            await botClient.SendMessage(chatId, "Я не понял ваш выбор. Напишите /start, чтобы начать.");
                            break;
                    }
                }
            }
        }



        async Task HandleStage1(ITelegramBotClient botClient, long chatId, string messageText)
        {
            _userStates[chatId] = "stage2";
            var replyMarkup = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton("Выбрать тропу \"Космос\""),
                    new KeyboardButton("Выбрать тропу \"Убийца\""),
                    new KeyboardButton("Выбрать тропу \"Водопад\""),
                })
            {
                ResizeKeyboard = true
            };
            await _sender.TrySendPhoto(chatId,
                "forkInTheRoad.png",
                replyMarkup,
                "В темные времена, когда магия и реальность переплетаются," +
                " существует одна загадка, которая не давала покоя самым лучшим детективам. " +
                "Это история об убийстве, которое никто не смог раскрыть — о таинственной туше и том, кто её убил." +
                " Легенда гласит, что разгадка этой тайны скрыта в трех путях, каждый из которых ведет к ключевым улик: " +
                "«Убийца», «Водопад» и «Космос». Но будьте осторожны: путь к истине нелегок," +
                " а самой главной угрозой является не только убийца, но и те, кто пытаются скрыть правду.");

        }

        async Task HandleStage2(ITelegramBotClient botClient, long chatId, string messageText)
        {
            if (messageText.ToLower() == "Выбрать тропу \"Космос\"")
            {
                _userStates[chatId] = "stage3";
                await botClient.SendMessage(chatId, "Вы победили дракона и вышли из леса! Поздравляю!");
            }
            else if (messageText.ToLower() == "использовать артефакт")
            {
                _userStates[chatId] = "stage3";
                await botClient.SendMessage(chatId, "Вы использовали артефакт и нашли выход из леса! Поздравляю!");
            }
            else
            {
                await botClient.SendMessage(chatId, "Пожалуйста, выберите: 'победить дракона' или 'использовать артефакт'.");
            }
        }

        async Task SendStage1(ITelegramBotClient botClient, long chatId)
        {
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

        async Task SendDragonChoices(ITelegramBotClient botClient, long chatId)
        {
            var replyMarkup = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton("Победить дракона"),
                    new KeyboardButton("Сбежать")
                })
            {
                ResizeKeyboard = true
            };

            await botClient.SendMessage(chatId, "Дракон грозно рычит. Что будете делать?", replyMarkup: replyMarkup);
        }

        async Task SendArtifactChoices(ITelegramBotClient botClient, long chatId)
        {
            var replyMarkup = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton("Использовать артефакт"),
                    new KeyboardButton("Изучить его поближе")
                })
            {
                ResizeKeyboard = true
            };

            await botClient.SendMessage(chatId, "Это магический артефакт. Что будете делать?", replyMarkup: replyMarkup);
        }

    }
}
