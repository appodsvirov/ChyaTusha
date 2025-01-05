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

            List<KeyboardButton> buttons = new();

            string sendMessage = "";
            MarkupBuilder builder = new MarkupBuilder().Add("🏠");

            if (messageText == "🏠")
            {
                await StartGameHandle(botClient, chatId, messageText);
                return;
            }
            else if (messageText == "Срачельник 🚽")
            {
                plot.BathroomState = 0;
                builder
                    .Add("Устроить помои")
                    .Add("Подарок 🎁");
                sendMessage = "";
            }
            else if (messageText == "Подарок 🎁")
            {
                plot.BathroomState = 1;
            }
            else if (messageText == "Устроить помои")
            {
                plot.BathroomState = 2;
                builder
                    .Add("Устроить помои")
                    .Add("Подарок 🎁");
                sendMessage = "";
            }

            await _sender.TrySendPhoto(chatId,
                    plot.Bathroom[plot.BathroomState],
                    sendMessage,
                    builder);
        }
    }
}
