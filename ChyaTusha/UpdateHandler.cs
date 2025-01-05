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
    public partial class UpdateHandler
    {
        private Dictionary<long, string> _userStates;
        private Dictionary<long, Plot> _userPlots;
        private Sender _sender;
        public UpdateHandler(Dictionary<long, string> userStates, Dictionary<long, Plot> userPlots, ITelegramBotClient botClient)
        {
            _userStates = userStates;
            _userPlots = userPlots;
            _sender = new(botClient);
        }

        //Start 
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message) return;

            long chatId = message.Chat.Id;
            var plot = _userPlots.TryGetValue(chatId, out var existingPlot) ? existingPlot : (_userPlots[chatId] = new());

            if (message.Text != null)
            {
                if (message.Text.ToLower() == "/start")
                {
                    // Начать квест
                    plot.Setup();
                    await IntroHandle(botClient, chatId);
                }
                else
                {
                    await Handle(botClient, chatId, message.Text);
                }
            }
        }

        public async Task Handle(ITelegramBotClient botClient, long chatId, string message)
        {
            string userState = _userStates.ContainsKey(chatId) ? _userStates[chatId] : "start";
            switch (userState)
            {
                case "StartGame":
                    await StartGameHandle(botClient, chatId, message);
                    break;
                case "Fork":
                    await ForkHandle(botClient, chatId, message);
                    break;
                case "Лес":
                    await ForestHandle(botClient, chatId, message);
                    break;
                case "Bathroom":
                    await BathroomHandle(botClient, chatId, message);
                    break;
                case "Водопад":
                    await WaterfallHandle(botClient, chatId, message);
                    break;
                case "Пещера":
                    await CaveHandle(botClient, chatId, message);
                    break;
                default:
                    await botClient.SendMessage(chatId, "Я не понял ваш выбор. Напишите /start, чтобы начать.");
                    break;
            }
        }

        async Task StartGameHandle(ITelegramBotClient botClient, long chatId, string messageText)
        {
            _userStates[chatId] = "Fork";
            var plot = _userPlots[chatId];
            MarkupBuilder builder = new MarkupBuilder();
            if (plot.IsKilled && !plot.HasWater)
            {
                builder.Add("💧");
                plot.HasWater = true;
            }

            builder
                .Add("Лес")
                .Add("Пещера")
                .Add("Водопад");

            var firstText = "В темные времена, когда магия и реальность переплетаются," +
                " существовала одна загадка, которая не давала покоя самым лучшим детективам. " +
                "Это история об убийстве, которое никто не смог раскрыть — о таинственной туше и том, кто её убил." +
                " Легенда гласит, что разгадка этой тайны скрыта в трех путях, каждый из которых ведет к ключевым уликам: " +
                "«Лес», «Водопад» и «Пещера». Но будьте осторожны: путь к истине нелегок," +
                " а самой главной угрозой является не только убийца, но и те, кто пытаются скрыть правду.";

            var secondText = "Вы стоите на краю неизвестности. Перед вами простираются три пути: лес, водопад и пещера." +
                "\nКаждый из них ведет к разгадке, но у каждого своя цена. " +
                "Лес скрывает следы, водопад уносит улики, а в пещере царит тьма, " +
                "в которой легко потеряться. Выбор — за вами.";

            await _sender.TrySendPhoto(chatId,
                "Fork.jpg",
                plot.IsIntroComplete ? secondText : firstText,
                builder);


            plot.IsIntroComplete = true;
        }

        async Task ForkHandle(ITelegramBotClient botClient, long chatId, string messageText)
        {
            var plot = _userPlots[chatId];
            if (messageText == "Лес")
            {
                _userStates[chatId] = "Лес";
            }
            else if (messageText == "Пещера")
            {
                _userStates[chatId] = "Пещера";
            }
            else if (messageText == "Водопад")
            {
                _userStates[chatId] = "Водопад";
            }
            else if (messageText == "💧")
            {
                plot.HasWater = true;
                await StartGameHandle(botClient, chatId, "");
                return;
            }
            else
            {
                _userStates[chatId] = null;
            }
            await Handle(botClient, chatId, messageText);
        }
    }
}
