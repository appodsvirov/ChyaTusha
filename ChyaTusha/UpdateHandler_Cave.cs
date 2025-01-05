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
        async Task CaveHandle(ITelegramBotClient botClient, long chatId, string messageText)
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
            else if (messageText == "Пещера")
            {
                plot.CaveState = 0;
                builder
                    .Add("Подарок 🎁");
                sendMessage = "";
            }
            else if (messageText == "Подарок 🎁")
            {
                plot.CaveState = 1;
                builder
                    .Add("Улика 💧");
                sendMessage = "";
            }
            else if (messageText == "Улика 💧")
            {
                plot.CaveState = 2;
                plot.HasSwaddle = true;

                sendMessage = "";
            }

            await _sender.TrySendPhoto(chatId,
                    plot.Cave[plot.CaveState],
                    sendMessage,
                    builder);
        }
    }
}
