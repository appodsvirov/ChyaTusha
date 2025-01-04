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
        async Task StartGame(ITelegramBotClient botClient, long chatId)
        {
            _userStates[chatId] = "StartGame";

            await _sender.TrySendPhoto(chatId, "StartGame.png",
                "Да начнется игра!😈");
        }
    }
}
