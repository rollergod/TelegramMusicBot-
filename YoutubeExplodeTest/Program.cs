using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeExplodeTest
{
   
    internal class Program
    {
        static TelegramBotClient bot = new TelegramBotClient(Constants.BOT_TOKEN);
        static Commands commands = new Commands(bot);
        static async Task Main(string[] args)
        {
            bot.StartReceiving();
            bot.OnMessage += Bot_OnMessage;
            Console.ReadLine();
            bot.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                var message = e.Message.Text;
                User user = bot.GetUpdatesAsync().Result[0].Message.From;
                commands.receivedCommand(e, message, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }    
        }
    }
}
