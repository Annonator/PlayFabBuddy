using Microsoft.Extensions.Configuration;
using PlayFabBuddy.PlayFabHelpers.Commands.Player;

namespace PlayFabBuddy.CreateOrLoginUsers
{
    public class Program
    {
        private int concurrentUsers;

        public static async Task<int> Main(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "-c", "concurrent" },
                { "-i", "input" },
                { "-o", "output" }
            };

            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args, switchMappings);


            var config = builder.Build();

            var pfConfig = new PlayFabHelpers.Admin.PlayFabConfig()
            {
                TitleId = "512DD",
                DeveloperSecret = "none"
            };

            pfConfig.InitAsync();

            var command = new RegisterNewPlayerCommand();

            await command.ExecuteAsync();

            if (config["concurrent"] == null)
            {
                Console.WriteLine("You have to define the Number of concurrent users!");
                Console.WriteLine("CreateOrLoginUsers -c <int>");
                Console.WriteLine("CreateOrLoginUsers --concurrent <int>");

                return 1;
            }


            //If there is no predifined User List to use, create random users!
            if(config["input"] == null)
            {
                /*var test = new LoginPlayerCommand()
                {

                };*/

            }

            if(config["concurrent"] != null)
            {
                Console.WriteLine($"Test!!!!! '{config["concurrent"]}'");
            }



            return 0;
        }
    }
}