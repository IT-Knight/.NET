using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net;


namespace TGBOT
{
    public partial class Handlers
    {
        static TelegramBotClient client = Program.client;
        public static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var Message = e.Message;
            var text = Message.Text;
            Console.WriteLine(text);
            var condition_default = text.StartsWith("/google@fuck_telegram_tester_bot");
            var condition_google_empty = text.StartsWith("/google") && text.Length <= 8;
            var condition_google = text.StartsWith("/google") && text.Length > 8;

            

            if (condition_default)
            {
                Console.WriteLine("condition_default");
                Message post_message = await client.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "<code>Инструкция по использованию:\n/google: текст поиска</code>",
                  parseMode: ParseMode.Html,
                  replyToMessageId: e.Message.MessageId
                  );

            }
            else if (condition_google_empty)
            {
                Console.WriteLine("condition_google_empty");
                Message post_message = await Program.client.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "Вы не ввели запрос для поиска!",
                  parseMode: ParseMode.Html,
                  replyToMessageId: e.Message.MessageId
                  );

            }
            else if (condition_google)
            {
                Console.WriteLine("condition_google");

                var search_text = text[8..].Trim();

                Database.DeleteOutdatedData();
                List<string> results = Database.GetFromDB(search_text);

                if (results.Count == 0)
                {
                    results = await Google.Search(search_text);
                    Database.AddResultsToDB(search_text, results);
                }

                
                

                InlineKeyboardMarkup keyboard = null;

                if (results.Count > 4)
                {
                    results = results.GetRange(0, 4);
                    keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Next", "Next|0|4") }); 
                }

                string post_text = string.Join("\n", results);

                Message post_message = await client.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: post_text,
                  disableWebPagePreview: true,
                  parseMode: ParseMode.Html,
                  replyToMessageId: e.Message.MessageId,
                  replyMarkup: keyboard);

                await Database.SaveLog(post_message, "PostMessage");
            }
        }
    }
}
