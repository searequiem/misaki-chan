using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiChan
{
    class MisakiBot
    {
        DiscordClient discord;
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

            var commands = discord.GetService<CommandService>();

            commands.CreateCommand("hello")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("whaddup, it's ya boi skinny penis");
                });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjMzNjgzMzk3NzU3NjMyNTE0.CthJ-g.qX-nRXra19WfVgfg2O4HG0GSXTM", TokenType.Bot);
            });
        }
        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
