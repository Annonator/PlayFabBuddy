using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Migration;

public class MigrateCommand : AsyncCommand<MigrateCommandSettings>
{
    public async override Task<int> ExecuteAsync(CommandContext context, MigrateCommandSettings settings)
    {
        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var tileDataProgressTask = ctx.AddTask("[yellow]Copy Title Data[/]", false);
            var tileInternalDataProgressTask = ctx.AddTask("[yellow]Copy Title Internal Data[/]", false);
            var tileCurrencyProgressTask = ctx.AddTask("[yellow]Copy Currency[/]", false);
            var tileFilesProgressTask = ctx.AddTask("[yellow]Copy Files[/]", false);
            var tileCatalogsProgressTask = ctx.AddTask("[yellow]Copy Catalogs[/]", false);
            var tileStoresProgressTask = ctx.AddTask("[yellow]Copy Stores[/]", false);
            var tileLegacyCloudScriptsProgressTask = ctx.AddTask("[yellow]Copy (Legacy) Cloud Scripts[/]", false);


        });

        return 0;
    }
}