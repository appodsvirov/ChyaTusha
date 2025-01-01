using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
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
        public async Task<bool> TrySendPhoto(long chatId, string name, string caption = "")
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
                        caption: caption
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
