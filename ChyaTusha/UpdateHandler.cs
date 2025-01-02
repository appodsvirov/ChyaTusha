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
            _userPlots[chatId] = new();

            if (message.Text != null)
            {
                if (message.Text.ToLower() == "/start")
                {
                    // Начать квест
                    await StartGame(botClient, chatId);
                }
                else
                {
                    Handle(botClient, chatId, message.Text);
                }
            }
        }

        public async void Handle(ITelegramBotClient botClient, long chatId, string message)
        {
            string userState = _userStates.ContainsKey(chatId) ? _userStates[chatId] : "start";
            switch (userState)
            {
                case "StartGame":
                    await HandleStage1(botClient, chatId, message);
                    break;
                case "Fork":
                    await HandleStage2(botClient, chatId, message);
                    break;
                case "Водопад":
                    await HandleStage3(botClient, chatId, message);
                    break;
                default:
                    await botClient.SendMessage(chatId, "Я не понял ваш выбор. Напишите /start, чтобы начать.");
                    break;
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
            Handle(botClient, chatId, messageText);
        }


        async Task HandleStage3(ITelegramBotClient botClient, long chatId, string messageText)
        {
            var plot = _userPlots[chatId];
            var replyMarkup = new ReplyKeyboardMarkup(
                    new[]
                    {
                    new KeyboardButton("Высушить 💦"),
                    new KeyboardButton("Подарок 💧"),
                    new KeyboardButton("Улика 💧"),
                    })
            {
                ResizeKeyboard = true
            };

            List<KeyboardButton> buttons = new();

            string message = "";

            if (messageText == "Водопад")
            {
                plot.WaterfallState = 0;
                message = "Водопад грохочет, скрывая улики за плотной завесой воды. " +
                    "Легенды гласят, что именно здесь были оставлены важнейшие подсказки, " +
                    "способные пролить свет на преступление. " +
                    "Но этот путь полон опасностей — поток водопада силён, и один неверный шаг может стать последним. " +
                    "Если осмелишься прыгнуть в водопад в поисках правды," +
                    " будь готов — глубины могут оказаться смертельно опасными. " +
                    "Утонуть здесь проще, чем найти истину";

            }
            else if(messageText == "Высушить 💦")
            {
                plot.WaterfallState = 1;
                message = "Вы стоите на краю водопада, вглядываясь в поток, " +
                    "скрывающий тайны прошлого. " +
                    "Шум воды заглушает мысли, но внутри растет ощущение, " +
                    "что ключ к разгадке — прямо здесь, на самом дне." +
                    "\nВы решаетесь. Снимаете с пояса древний артефакт — Пеленка бесконечности\n" +
                    "Легкое движение, и водопад полностью впитываются в пеленку \n" +
                    "Поток воды медленно исчезает, словно подчиняясь вашему желанию. " +
                    "Вскоре на месте бурлящего водопада остается лишь спокойная, сухая расщелина." +
                    "\nИ вот оно — на самом дне, в окружении гладких камней, лежит коробка. " +
                    "Она выглядит так, словно кто-то спрятал его здесь давным-давно. " +
                    "Подняв ее, вы замечаете, что это — подарок, перевязанный красной лентой. " +
                    "На нем записка:" +
                    "\n«Тому, кто осмелится дойти до конца. Открой, когда почувствуешь, что пора.»" +
                    "\nПодарок теплый на ощупь, и от него исходит слабое свечение. Вам предстоит решить, открыть его сейчас… или позже.";
            }
            else if (messageText == "Подарок 💧")
            {
                plot.WaterfallState = 2;
            }
            else if (messageText == "Улика 💧")
            {
                plot.WaterfallState = 3;
            }

            await _sender.TrySendPhoto(chatId,
                    plot.Waterfall[plot.WaterfallState],
                    replyMarkup,
                    message
                    );

        }
    }
}
