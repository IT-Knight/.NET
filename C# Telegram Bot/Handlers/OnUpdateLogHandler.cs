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
        public static async void OnPreProccessUpdate(object sender, UpdateEventArgs e)
        {
            try
            {
                var message = e.Update.Message;

                if (message == null || message.Type != MessageType.Text) 
                { 
                    return; 
                }

                string text = message.Text;

                if (text.Length > 0)
                {
                    if (text.StartsWith("/google:") || text.StartsWith("/google")) {
                        await Database.SaveLog(message, "Update"); // simplified
                    } else {
                        await Database.SaveLog(message, "OtherInput");
                    }
                }    
            } catch (Exception ex){
                Console.WriteLine("OnUpdateHandler. Error: " + ex);
            }
            
        }
    }
}
