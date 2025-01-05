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
        async Task WaterfallHandle(ITelegramBotClient botClient, long chatId, string messageText)
        {
            var plot = _userPlots[chatId];

            List<KeyboardButton> buttons = new();
            MarkupBuilder builder = new MarkupBuilder().Add("🏠");

            string sendMessage = "";

            if (messageText == "🏠")
            {
                await StartGameHandle(botClient, chatId, messageText);
                return;
            }
            else if (messageText == "Водопад")
            {
                plot.WaterfallState = 0;

                if (plot.HasSwaddle)
                {
                    builder.Add("Высушить 💨");
                }

                sendMessage = "Водопад грохочет, скрывая улики за плотной завесой воды. " +
                    "Легенды гласят, что именно здесь были оставлены важнейшие подсказки, " +
                    "способные пролить свет на преступление. " +
                    "Но этот путь полон опасностей — поток водопада силён, и один неверный шаг может стать последним. " +
                    "Если осмелишься прыгнуть в водопад в поисках правды," +
                    " будь готов — глубины могут оказаться смертельно опасными. " +
                    "Утонуть здесь проще, чем найти истину";
            }
            else if (messageText == "Высушить 💨")
            {
                plot.WaterfallState = 1;
                builder
                    .Add("Подарок 🎁");
                sendMessage = "Вы стоите на краю водопада, вглядываясь в поток, " +
                    "скрывающий тайны прошлого. " +
                    "Шум воды заглушает мысли, но внутри растет ощущение, " +
                    "что ключ к разгадке — прямо здесь, на самом дне." +
                    "\nВы решаетесь. Снимаете с пояса древний артефакт — Пеленку бесконечности\n" +
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
            else if (messageText == "Подарок 🎁")
            {
                plot.WaterfallState = 2;
                builder
                    .Add("Улика 💧");
            }
            else if (messageText == "Улика 💧")
            {
                plot.WaterfallState = 3;
                plot.HasBags = true;
                plot.CaveState++;
            }
            else
            {
                _userStates[chatId] = null;
            }

            await _sender.TrySendPhoto(chatId,
                    plot.Waterfall[plot.WaterfallState],
                    sendMessage,
                    builder
                    );

        }
    }
}
