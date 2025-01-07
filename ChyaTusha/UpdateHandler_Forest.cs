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
                if (!plot.IsIntroForestComplete)
                {
                    sendMessage = "Когда вы вступаете в лес, воздух вокруг становится плотнее." +
                        "\nКоричневый газ медленно стелется по земле, окутывая тропы и скрывая их от глаз. " +
                        "Лес кажется безмолвным, но каждый шаг отдается в тишине эхом, будто за вами кто-то следит." +
                        "\nЭтот лес живет своей жизнью. Его дороги извилисты и коварны. Лишь тот, кто пройдет по правильному пути, " +
                        "найдет \"Кучу Любви\" — древний артефакт, скрывающийся за завесой иллюзий и дурных запахов. " +
                        "Легенды гласят, что путники, которые находят эту кучу и собирают её, " +
                        "в этот день встречают любовь всей своей жизни. Но будьте осторожны: неверный шаг," +
                        " и лес вернет вас в самое начало...\nВ воздухе раздается тихий смешок — кажется, сам лес развлекается.";
                }
                else
                {
                    sendMessage = "Лес терпелив. Он будет возвращать вас в самое начало столько раз," +
                        " сколько потребуется, пока вы не найдете единственный верный путь. " +
                        "Куча Любви ждет, но лишь тот, кто проявит настойчивость и чистое сердце, сможет её заполучить.";
                    plot.IsIntroForestComplete = true;
                }
                    
            }
            else if (plot.ShitForestState == 0 && messageText == "←")
            {
                plot.ShitForestState = 1;
                builder
                    .Add("←")
                    .Add("↓");
            }
            else if (plot.ShitForestState == 1 && messageText == "↓")
            {
                plot.ShitForestState = 2;
                builder
                    .Add("←")
                    .Add("→")
                    .Add("↓");
            }
            else if (plot.ShitForestState == 2 && messageText == "→")
            {
                plot.ShitForestState = 3;
                builder
                    .Add("↓");
            }
            else if (plot.ShitForestState == 3 && messageText == "↓")
            {
                plot.ShitForestState = 4;
                builder
                    .Add("←")
                    .Add("↓");
            }
            else if (plot.ShitForestState == 4 && messageText == "←")
            {
                plot.ShitForestState = 5;
                builder
                    .Add("←")
                    .Add("↓");
                sendMessage = "Перед вами открывается развилка, и лес становится темнее, как будто предлагает вам выбор. " +
    "Тропы расходятся в разные стороны, и каждый путь кажется полным загадок";
            }
            else if (plot.ShitForestState == 5 && messageText == "↓")
            {
                plot.ShitForestState = 6;
                builder
                    .Add("←")
                    .Add("↓");


            }
            else if (plot.ShitForestState == 6 && messageText == "←")
            {
                plot.ShitForestState = 7;

                if (plot.HasBags)
                {
                    builder
                        .Add("Улика 💩");
                    sendMessage = "Вы стоите перед Кучей Любви. Её сияние... в меру ослепительно." +
                        "\nНастало время. Рулон Чистоты Героев в ваших руках — и лес признаёт вас достойным. " +
                        "Соберите Кучу с уважением, и путь вперед откроется сам собой." +
                        "\nКак ни странно, газ вокруг начинает рассеиваться, будто лес с гордостью отпускает вас дальше.";
                }
                else
                {
                    builder.Add("🏠");
                    sendMessage = "Вы стоите перед Кучей Любви. Её аромат... неповторим." +
                        "\nВот она — цель вашего пути. Но без Рулона Чистоты Героев вы не сможете её взять. " +
                        "Лес не потерпит неуважения к столь ценному артефакту.\nКуча словно смотрит на вас в ожидании… " +
                        "или вам просто так кажется";
                }
            }

            else if (plot.ShitForestState == 7 && messageText == "Улика 💩")
            {
                plot.ShitForestState = 8;
                plot.CaveState = 4;
                plot.HasShit = true;
                builder
                    .Add("Подарок 🎁");
                sendMessage = "Только вы поднимаете Кучу Любви, как на её месте появляется аккуратно перевязанный подарок." +
                    "\nЛес решил отблагодарить вас за старания. Возможно, внутри скрыто нечто, что поможет в дальнейших испытаниях. " +
                    "Хотите взять подарок?" +
                    "\nПодарок лежит прямо перед вами, словно ждет, когда вы протянете руку.";
            }
            else if (plot.ShitForestState == 8 && messageText == "Подарок 🎁")
            {
                plot.ShitForestState = 9;
                builder
                    .Add("Срачельник 🚽");
                sendMessage = "Как только вы берете подарок, лес словно замирает. Газ исчезает, оставив за собой чистый воздух." +
                    " Дорога перед вами открыта, и вы замечаете, что всё вокруг стало тихим и пустым." +
                    "\nГаз расселся, и тропа, скрытая от глаз, теперь ясна. " +
                    "Похоже, рядом с Кучей Любви должен быть и Срачельник. Пора двигаться дальше." +
                    "\nВы чувствуете, как лес уже не сдерживает вас, предоставляя шанс идти вперед.";


                await botClient.SendMessage(chatId,
                    "*Загадка #3:*\n" +
                    "Там, где отдыхает хранитель границ дома, под его покоем скрыто то, " +
                    "что может обновить не только силы, но и ход событий. " +
                    "Взгляд твой пройдет мимо, если не вспомнишь, что иногда ответ лежит у самых лап", 
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
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
