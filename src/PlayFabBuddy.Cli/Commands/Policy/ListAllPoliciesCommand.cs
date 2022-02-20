using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Policy;

public class ListAllPoliciesCommand : Command<PolicySettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] PolicySettings settings)
    {
        // For now hardcode this as we only have 2 policies 
        var table = new Table();

        table.AddColumn("Policy Name");
        table.AddColumn("Description");

        table.AddRow("[green]AllowCustomLogin[/]", "This allows you to enable client login with CustomId");
        table.AddRow("[green]DenyCustomLogin[/]", "This will block client from login in with CustomId");

        AnsiConsole.Write(table);

        return 0;
    }
}
