using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static ChyaTusha.ExternalEnvironmentHandler;

namespace ChyaTusha
{
    public class Sender
    {
        ITelegramBotClient _botClient;
        public Sender(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        public async Task<bool> TrySendPhoto(long chatId, string name, IReplyMarkup replyMarkup, string caption = "")
        {
            try
            {
                using (var stream = new FileStream(Path.Combine(GetResourceFolderPath(), name)
                    , FileMode.Open))
                {
                    var input = new InputFileStream(stream);

                    // Отправка фото
                    await _botClient.SendPhoto(
                        chatId: chatId,
                        photo: input,
                        caption: caption,
                        replyMarkup: replyMarkup
                    );
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
