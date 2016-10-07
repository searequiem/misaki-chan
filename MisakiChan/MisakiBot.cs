using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiChan
{
    class MisakiBot
    {
        DiscordClient discord;
        CommandService commands;
        Random rand;
        String[] randomPkmnFromText;

        public MisakiBot()
        {
            
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '>';
                x.AllowMentionPrefix = true;
            });

            commands = discord.GetService<CommandService>();

            randomPokemon();

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("", TokenType.Bot);
            });
        }

        private void randomPokemon() 
        {
            commands.CreateCommand("pkmn")
                .Do(async (e) =>
                 {
                     rand = new Random();
                     randomPkmnFromText = File.ReadAllLines("pokemon.txt");
                     string pokemonToPost = randomPkmnFromText[rand.Next(randomPkmnFromText.Length)];
                     string bulbapediaLink = "http://bulbapedia.bulbagarden.net/wiki/" + pokemonToPost;
                     await e.Channel.SendMessage(bulbapediaLink);
                 });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
