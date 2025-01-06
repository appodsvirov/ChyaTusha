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
            MarkupBuilder builder = new MarkupBuilder();


            if (messageText == "🏠")
            {
                await StartGameHandle(botClient, chatId, messageText);
                return;
            }
            else if (messageText == "Пещера" && plot.CaveState == 0)
            {
                builder
                    .Add("Подарок 🎁");
                sendMessage = "Пещера встречает вас зловещей тишиной. Из темноты выглядывают два желтых глаза —" +
                    " неподвижные, но внимательные. Они словно ждут... " +
                    "или предупреждают. Прежде чем ступить внутрь, убедитесь, что знаете правду: кем является убийца и кого он убил. " +
                    "Но помните: убийца не прощает ошибок." +
                    "У входа в пещеру лежит небольшая коробка, словно оставленная кем-то в спешке." +
                    " Может, это ловушка… или подсказка, которая поможет вам узнать правду о том, " +
                    "что скрывает эта тьма.";
            }
            else if (messageText == "Подарок 🎁")
            {
                plot.CaveState = 1;
                builder
                    .Add("Улика 💧");
                sendMessage = "Вы находите Пеленку Бесконечности — древний артефакт, способный укрыть вас от любой стихии. " +
                    "Легенды гласят, что даже потоки воды не могут прорваться сквозь её волокна. " +
                    "Возможно, однажды это спасет вам жизнь.";
            }
            else if (messageText == "Улика 💧")
            {
                plot.CaveState = 2;
                plot.HasSwaddle = true;

                sendMessage = "Когда вы поднимаете Пеленку Бесконечности, пространство вокруг пещеры замирает.\n" +
                    "Теперь здесь больше нечего искать. Но знайте: с каждой найденной уликой убийца приближается к выходу. Он терпелив… но не вечен. Время работает против вас." +
                    "\nТишина становится тяжелой, словно давит на плечи. Пора идти дальше.";
            }
            else
            {
                _userStates[chatId] = null;
            }


            if (plot.CaveState >= 2)
            {
                builder.Add("🏠");
            }


            await _sender.TrySendPhoto(chatId,
                    plot.Cave[plot.CaveState],
                    sendMessage,
                    builder);
        }
    }
}
