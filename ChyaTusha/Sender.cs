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
        public async Task<bool> TrySendPhoto(long chatId, string name, string caption, params string[] markupTexts)
        {
            var builder = new MarkupBuilder().AddRange(markupTexts);
            return await InternalSendPhoto(chatId, name, caption, builder);
        }

        public async Task<bool> TrySendPhoto(long chatId, string name, string caption, MarkupBuilder builder)
        {
            return await InternalSendPhoto(chatId, name, caption, builder);
        }

        private async Task<bool> InternalSendPhoto(long chatId, string name, string caption, MarkupBuilder builder)
        {
            try
            {
                using (var stream = new FileStream(Path.Combine(GetResourceFolderPath(), name), FileMode.Open))
                {
                    var input = new InputFileStream(stream);

                    await _botClient.SendPhoto(
                        chatId: chatId,
                        photo: input,
                        caption: caption,
                        replyMarkup: builder.Build(),
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                    );
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка в отправке фото {name}:\n{e.Message}\n");
                return false;
            }
        }


        public async Task<bool> SendVoiceMessage(long chatId, string fileName, string caption = "")
        {
            try
            {
                string filePath = Path.Combine(GetResourceFolderPath(), fileName);

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var input = new InputFileStream(stream);

                    await _botClient.SendVoice(
                        chatId: chatId,
                        voice: input,
                        caption: caption
                    );
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при отправке голосового сообщения {fileName}:\n{e.Message}\n");
                return false;
            }
        }

    }
}
