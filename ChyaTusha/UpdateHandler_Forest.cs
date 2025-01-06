using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace ChyaTusha
{
    public partial class UpdateHandler
    {
        async Task ForestHandle(ITelegramBotClient botClient, long chatId, string messageText)
        {
            var plot = _userPlots[chatId];

            List<KeyboardButton> buttons = new();

            string sendMessage = "";
            MarkupBuilder builder = new MarkupBuilder();

            if (plot.HasShit && plot.ShitForestState == 9)
            {
                _userStates[chatId] = "Bathroom";
                await Handle(botClient, chatId, "Срачельник 🚽");
                return;
            }
            else if (messageText == "🏠")
            {
                await StartGameHandle(botClient, chatId, messageText);
                return;
            }
            else if (messageText == "Лес")
            {
                plot.ShitForestState = 0;
                builder.Add("🏠");
                builder
                    .Add("←")
                    .Add("↓");

                sendMessage = "";
            }
            else if (plot.ShitForestState == 0 && messageText == "←")
            {
                plot.ShitForestState = 1;
                builder
                    .Add("←")
                    .Add("↓");

                sendMessage = "1";
            }
            else if (plot.ShitForestState == 1 && messageText == "↓")
            {
                plot.ShitForestState = 2;
                builder
                    .Add("←")
                    .Add("→")
                    .Add("↓");

                sendMessage = "2";
            }
            else if (plot.ShitForestState == 2 && messageText == "→")
            {
                plot.ShitForestState = 3;
                builder
                    .Add("↓");

                sendMessage = "3";
            }
            else if (plot.ShitForestState == 3 && messageText == "↓")
            {
                plot.ShitForestState = 4;
                builder
                    .Add("←")
                    .Add("↓");

                sendMessage = "4";
            }
            else if (plot.ShitForestState == 4 && messageText == "←")
            {
                plot.ShitForestState = 5;
                builder
                    .Add("←")
                    .Add("↓");

                sendMessage = "5";
            }
            else if (plot.ShitForestState == 5 && messageText == "↓")
            {
                plot.ShitForestState = 6;
                builder
                    .Add("←")
                    .Add("↓");

                sendMessage = "6";
            }
            else if (plot.ShitForestState == 6 && messageText == "←")
            {
                plot.ShitForestState = 7;

                if (plot.HasBags)
                {
                    builder
                        .Add("Улика 💩");
                    sendMessage = "8";
                }
                else
                {
                    sendMessage = "";
                }
            }

            else if (plot.ShitForestState == 7 && messageText == "Улика 💩")
            {
                plot.ShitForestState = 8;
                plot.CaveState++;
                plot.HasShit = true;
                builder
                    .Add("Подарок 🎁");
                sendMessage = "9";
            }
            else if (plot.ShitForestState == 8 && messageText == "Подарок 🎁")
            {
                plot.ShitForestState = 9;
                builder
                    .Add("Срачельник 🚽");
                sendMessage = "10";
            }
            else if (messageText == "Срачельник 🚽")
            {
                _userStates[chatId] = "Bathroom";
                await Handle(botClient, chatId, messageText);
                return;
            }
            else
            {
                await ForestHandle(botClient, chatId, "Лес");
                return;
            }

            await _sender.TrySendPhoto(chatId,
                    plot.ShitForest[plot.ShitForestState],
                    sendMessage,
                    builder
                    );

        }
    }
}
