using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChyaTusha;
using ChyaTusha.Extensions;
using static ChyaTusha.ExternalEnvironmentHandler;

string pathToKey = Path.Combine(GetOutputFolderPath(), @"key.txt");

if (!System.IO.File.Exists(pathToKey))
{
    Console.WriteLine("Введите токен");
    var line = Console.ReadLine().ToString();

    Directory.CreateDirectory(pathToKey.GetDirectory());
    System.IO.File.WriteAllText(pathToKey, line);
}

var key = System.IO.File.ReadAllText(pathToKey);
try
{
    var botClient = new TelegramBotClient(key);
    var userStates = new Dictionary<long, string>();
    var handler = new UpdateHandler(userStates, botClient);

    botClient.StartReceiving(
        updateHandler: handler.HandleUpdateAsync,
        errorHandler: HandleErrorAsync
    );

    Console.WriteLine("Бот запущен. Нажмите любую клавишу для выхода...");
    Console.ReadKey();
}
catch(ArgumentException e)
{
    Console.WriteLine(e.Message);
    System.IO.File.Delete(pathToKey);
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine($"Ошибка: {exception.Message}");
    return Task.CompletedTask;
}
