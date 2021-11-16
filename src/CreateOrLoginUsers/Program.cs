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
            builder.AddJsonFile("local.settings.json");

            var config = builder.Build();

            if(config["devSecret"] == null || config["titleId"] == null)
            {
                Console.WriteLine("Could not load PlayFab TitleId and Developer Secret from local.appsettings.json");

                return 1;
            }

            var pfConfig = new PlayFabHelpers.Admin.PlayFabConfig()
            {
                TitleId = config["titleId"],
                DeveloperSecret = config["devSecret"]
            };

            pfConfig.InitAsync();

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