using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeExplodeTest
{
    public class Commands
    {
        private TelegramBotClient Bot { get; }
        public static YoutubeClient youtubeClient = new YoutubeClient();

        public Commands(TelegramBotClient _bot)
        {
            Bot = _bot;
        }
        public void receivedCommand(MessageEventArgs e,string message,User user)
        {
            try
            {
                if(!checkCommand(message) && !message.StartsWith(Constants.COMMAND_EXAMPLE))
                {
                    commandRandom(e, user);
                    return;
                }
                if (message.StartsWith(Constants.COMMAND_HELP))
                    commandHelp(e, user);
                else if (message.StartsWith(Constants.COMMAND_EXAMPLE))
                    getMusic(e);
                else if (message.StartsWith(Constants.COMMAND_MUSIC))
                    commandMusic(e, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }    
        }
        private void commandHelp(MessageEventArgs e, User user)
        {
            Console.WriteLine(user.Username + " used help command");
            Bot.SendTextMessageAsync(e.Message.Chat.Id,
                "# HELP #\n\nHere's a list of commands:\n/help - show list of commands\n/about - about bot and creator\n/music - request bot to search music\n/ping - get the server ping");
        }

        private void commandMusic(MessageEventArgs e, User user)
        {
            Console.WriteLine(user.Username + " used music command");
            Bot.SendTextMessageAsync(e.Message.Chat.Id,
                "# Music #\n\n To find the music -  use /music (VIDEO URL FROM YouTube)\n Example - /music https://www.youtube.com/watch?v=86H1Ln6e2Lg");
        }

        private async Task getMusic(MessageEventArgs e)
        {
            string videolURL = e.Message.Text.Substring(Constants.COMMAND_MUSIC.Length);

            var video = await youtubeClient.Videos.GetAsync(videolURL);
            Console.WriteLine($"Название: {video.Title}");
            Console.WriteLine($"Продолжительность: {video.Duration}");
            Console.WriteLine($"Автор: {video.Author}");

            StreamManifest streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(videolURL);
            var streamInfo = (AudioOnlyStreamInfo)streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();


            var stream = await youtubeClient.Videos.Streams.GetAsync(streamInfo);

            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream);
            await Bot.SendAudioAsync(e.Message.Chat.Id, inputOnlineFile, "Лови", ParseMode.Default, 
                performer: video.Title, title: video.Author.ToString(), duration: (int)video.Duration.Value.TotalSeconds);
        }

        private void commandRandom(MessageEventArgs e, User user)
        {
            Console.WriteLine(user.Username + " used random command");
            Bot.SendTextMessageAsync(e.Message.Chat.Id,
                "I dont know this command. Try this - /help to get info about the commands");
        }

        private bool checkCommand(string command)
        {
            bool isCommandCoorect = false;
            switch (command)
            {
                case Constants.COMMAND_HELP:
                    isCommandCoorect = true;
                    break;
                case Constants.COMMAND_MUSIC:
                    isCommandCoorect = true;
                    break;
                default:
                    break;
            }

            return isCommandCoorect;
        }
    }
}
