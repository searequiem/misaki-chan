using Discord;
using Discord.Commands;
using System;
using System.IO;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Customsearch.v1;

namespace MisakiChan
{

    public class MisakiBot
    {
        private string DiscordAPI = "MjMzNzQ3NDIxNTAxMzI1MzEy.CtiLIA.nZDPPRCOkLEpuJtTwRvoe86nzFY";
        private string GoogleAPI = "AIzaSyA71-cLSBJ3CYi06nEff6IDWVKr0KjJAHE";
        private string pokemonSearchEngID = "014488993494222492713:ivwpsosfc2w";
        public DiscordClient discord;
        public CommandService commands;
        public CustomsearchService pokemonSearch;
        private ConsoleSpiner spin = new ConsoleSpiner();
        public Random rand;
        public String[] randomPkmnFromText;
        public string googleSearchQuery;
        public string firstLink;
        public string firstImage;

        public MisakiBot()
        {                                       /* Defining our variables! (ノ*ﾟ▽ﾟ*) */
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

                                                /* Setting our prefix character for commands! (ノ*ﾟ▽ﾟ*) */
            discord.UsingCommands(x =>  
            {
                x.PrefixChar = '>';
                x.AllowMentionPrefix = true;
            });

            commands = discord.GetService<CommandService>();

            commandStart();

            int lBuffer = 1;
            Console.Write("Working....");
            while (lBuffer < 30000)
            {
                spin.Turn();
                lBuffer++;
            }
            Console.WriteLine("!");

            Console.WriteLine("Good morning!");

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(DiscordAPI, TokenType.Bot);
            });
        }

        public void commandStart()
        {           //POKEMON COMMAND
            commands.CreateCommand("pkmn")
            
                .Do(async (e) =>
                {
                    Console.WriteLine("Pokemon command executed.");
                    //Defining variables for this command. <3 
                    rand = new Random();
                    randomPkmnFromText = File.ReadAllLines("pokemon.txt");
                    string pokemonToPost = randomPkmnFromText[rand.Next(randomPkmnFromText.Length)];
                    string googleSearchQuery = pokemonToPost;
                    pokemonSearch = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer() { ApiKey = GoogleAPI });
                    CseResource.ListRequest listRequest = pokemonSearch.Cse.List(googleSearchQuery);
                    listRequest.Cx = pokemonSearchEngID;
                    Search search = listRequest.Execute();

                    //Executing the code for this command. <3
                    foreach (var item in search.Items) {
                        if (item.DisplayLink == item.DisplayLink)
                        {
                            //Brian McCool is a real ass motherfucker, and don't you ever forget that.
                            Console.WriteLine("Pokemon found.");
                            firstLink = ("🌸 " + pokemonToPost + ": " + item.Link + " 🌸");
                            //firstImage = (item.Image.ThumbnailLink);
                            break;
                        }
                    }
                    await e.Channel.SendMessage(firstLink);
                    Console.WriteLine("Pokemon link sent successfully.");
                    //await e.Channel.SendFile(firstImage);
                    //Console.WriteLine("Pokemon image sent successfully.");
                });

                    //IMPLYING COMMAND
            commands.CreateCommand("implying")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("implications");
                });

                    //PURGE COMMAND
            commands.CreateCommand("purge")
               .Parameter("purgeAmount")
               .Do(async e =>
               {
                   var messagesToDelete = await e.Channel.DownloadMessages(Convert.ToInt32(e.GetArg("purgeAmount")));
                   await e.Channel.DeleteMessages(messagesToDelete);
                   await e.Channel.SendMessage(e.GetArg("purgeAmount") + " Messages Deleted. (⁄ ⁄>⁄ ▽ ⁄<⁄ ⁄)");
                   Console.WriteLine(e.GetArg("purgeAmount") + " Messages Deleted.");
               });
        }

        public class ConsoleSpiner
        {
            int counter;
            public ConsoleSpiner()
            {
                counter = 0;
            }
            public void Turn()
            {
                counter++;
                switch (counter % 4)
                {
                    case 0: Console.Write("/"); break;
                    case 1: Console.Write("-"); break;
                    case 2: Console.Write("\\"); break;
                    case 3: Console.Write("|"); break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
