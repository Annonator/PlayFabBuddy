using Microsoft.Extensions.Configuration;
using PlayFabBuddy.PlayFabHelpers.Commands.Player;
using PlayFabBuddy.PlayFabHelpers.Util.IoC;

namespace PlayFabBuddy.CreateOrLoginUsers
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "-c", "concurrent" },
                { "-i", "input" },
                { "-o", "output" }
            };

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("settings.json");
            builder.AddJsonFile("local.settings.json", true);
            builder.AddCommandLine(args, switchMappings);

            var config = builder.Build();

            if (config["devSecret"] == null || config["titleId"] == null)
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

            DependencyInjection.Instance.Register<PlayFabBuddy.PlayFabHelpers.Util.Config.IConfig>(() => pfConfig, RegistrationType.Singleton);

            int concurrentUsers;

            if (config["concurrent"] == null || !int.TryParse(config["concurrent"], out concurrentUsers))
            {
                Console.WriteLine("You have to define the Number of concurrent users!");
                Console.WriteLine("CreateOrLoginUsers -c <int>");
                Console.WriteLine("CreateOrLoginUsers --concurrent <int>");

                return 1;
            }

            Console.WriteLine("Starting " + concurrentUsers + " Tasks to run concurrent....\n");

            List<Task> commands = new List<Task>();

            for (int i = 0; i < concurrentUsers; i++)
            {
                Console.Write(".");

                commands.Add(new RegisterNewPlayerCommand().ExecuteAsync());
            }

            await Task.WhenAll(commands);

            //If there is no predifined User List to use, create random users!
            if (config["input"] == null)
            {
                var createUsers = new RegisterNewPlayerCommand();

            }

            return 0;
        }
    }
}