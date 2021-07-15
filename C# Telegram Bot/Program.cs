using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Configuration;
using System.Net;

namespace TGBOT
{
    public class Program
    {
        private static string Token = ConfigurationManager.AppSettings["Token"];
        public static TelegramBotClient client;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(Token);

            var commands_list = new List<BotCommand> { new BotCommand() { Command = "google", Description = "/google: < текст поиска >" } };
            client.SetMyCommandsAsync(commands_list);

            client.OnMessage += Handlers.OnMessageHandler;
            client.OnCallbackQuery += Handlers.OnCallBackHandler;
            client.OnUpdate += Handlers.OnPreProccessUpdate;

            var checkMainTable_exists = new Database();

            client.StartReceiving();
            Console.WriteLine("Бот Запущен и готов к работе");
            client.SendTextMessageAsync("1208151003", "Бот Запущен и готов к работе");

            System.Threading.Thread.Sleep(-1);  // Non stop

            client.StopReceiving();
        }


    }
}
