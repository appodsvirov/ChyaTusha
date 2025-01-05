using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChyaTusha
{
    public partial class UpdateHandler
    {
        async Task BathroomHandle(ITelegramBotClient botClient, long chatId, string messageText)
        {
            var plot = _userPlots[chatId];
            string sendMessage = "";
            MarkupBuilder builder = new MarkupBuilder().Add("🏠");

            if (messageText == "🏠")
            {
                await StartGameHandle(botClient, chatId, messageText);
                return;
            }
            else if (messageText == "Срачельник 🚽")
            {
                if (plot.HasWater)
                {
                    builder.Add("Устроить помои");
                }
                builder.Add("Подарок 🎁");
                sendMessage = "";
            }
            else if (messageText == "Подарок 🎁" && !plot.HasWater)
            {
                plot.BathroomState = 1;
                plot.IsKilled = true;
            }
            else if (messageText == "Устроить помои")
            {
                plot.BathroomState = 2;
                builder
                    .Add("Подарок 🎁");
                sendMessage = "";
            }
            else if (messageText == "Подарок 🎁" && plot.HasWater)
            {
                plot.BathroomState = 3;
                builder
                    .Add("Улика 🛏️");
                sendMessage = "";
            }
            else if (messageText == "Улика 🛏️")
            {
                plot.BathroomState = 4;
                plot.CaveState++;
            }
            else
            {
                _userStates[chatId] = null;
            }

            await _sender.TrySendPhoto(chatId,
                    plot.Bathroom[plot.BathroomState],
                    sendMessage,
                    builder);

            if (plot.BathroomState == 1)
            {
                plot.BathroomState = 0;
            }
        }
    }
}
