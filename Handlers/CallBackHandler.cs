using System;
using System.Collections.Generic;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace TGBOT
{
    partial class Handlers
    {
        public static async void OnCallBackHandler(object sender, CallbackQueryEventArgs e)
        {
            string callback_data = e.CallbackQuery.Data;
            Console.WriteLine(callback_data);
            var list3 = callback_data.Split("|");
            string option = list3[0];
            int start = Int32.Parse(list3[1]);
            int end = Int32.Parse(list3[2]);

            var raw_text = e.CallbackQuery.Message.ReplyToMessage.Text;
            Console.WriteLine(raw_text);
            var search_text = raw_text.Substring(8, raw_text.Length - 8).Trim();
            Console.WriteLine(search_text);

            Database.DeleteOutdatedData();

            List<string> text_list = Database.GetFromDB(search_text);  // достаем из БД результаты поиска MongoDB().getFromDB(search_text)

            switch (option)
            {
                case "Next":
                    start += 4; // (0, 4, 8)
                    if (end == 8) { end += 2; } else { end += 4; }  // (4, 8 , 12->10)
                    break;
                case "Back":
                    start -= 4;
                    if (end == 10) { end -= 2; } else { end -= 4; };
                    break;
            }
            Console.WriteLine("start: {0} end: {1}", start, end);

            InlineKeyboardMarkup keyboard = null;

            if (3 < start && start < 8)
            { keyboard = new InlineKeyboardMarkup(
                new InlineKeyboardButton[][]
                {
                        new [] { new InlineKeyboardButton() { Text = "Back", CallbackData = $"Back|{start}|{end}" },
                                 new InlineKeyboardButton() { Text = "Next", CallbackData = $"Next|{start}|{end}" } }
                });
            } else if (start <= 3)
            { keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton { Text = "Next", CallbackData = $"Next|{start}|{end}" }); }
            else if (start >= 8)
            { keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton { Text = "Back", CallbackData = $"Back|{start}|{end}" }); }

            if (text_list.Count != 0) {
                string post_text = String.Join("\n", text_list.GetRange(start, end - start));
                Console.WriteLine("Callback No Rewrite");

                Message post_message = await client.EditMessageTextAsync(
                    chatId: e.CallbackQuery.Message.Chat,
                    messageId: e.CallbackQuery.Message.MessageId,
                    text: post_text,
                    disableWebPagePreview: true,
                    parseMode: ParseMode.Html,
                    replyMarkup: keyboard);

                await Database.SaveLog(post_message, "PostMessage");

            } else {
                var results = await Google.Search(search_text);

                Database.AddResultsToDB(search_text, results);

                Console.WriteLine($"res count: {results.Count}");

                string post_text = "";

                if (results.Count != 0)
                {
                    post_text = String.Join("\n", results.GetRange(start, end - start));
                } else
                {
                    throw new Exception(message: $"search_text: {search_text} \n results = 0");
                }


                // await save results in DB 

                //keyboard = Keyboards.keyboard_start;
                Console.WriteLine("Callback Else: rewrite in DB");

                Message post_message = await client.EditMessageTextAsync(
                    chatId: e.CallbackQuery.Message.Chat,
                    messageId: e.CallbackQuery.Message.MessageId,
                    text: post_text,
                    disableWebPagePreview: true,
                    parseMode: ParseMode.Html,
                    replyMarkup: keyboard);
                
                await Database.SaveLog(post_message, "PostMessage");

            } 
        }
    }
}
