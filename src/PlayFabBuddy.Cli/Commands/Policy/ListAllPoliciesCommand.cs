using System.Diagnostics.CodeAnalysis;
using PlayFabBuddy.Lib.Entities.Policy;
using PlayFabBuddy.Lib.UseCases.Policy;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Policy;

public class ListAllPoliciesCommand : AsyncCommand<ListAllPoliciesCommandSettings>
{
    private readonly ListAllUseCase _listAllUseCase;

    public ListAllPoliciesCommand(ListAllUseCase listAllUseCase)
    {
        _listAllUseCase = listAllUseCase;
    }

    public async override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] ListAllPoliciesCommandSettings settings)
    {

        if (settings.IsRemote == true)
        {
            var remoteTable = new Table();
            // Add some columns
            remoteTable.AddColumn("Policy Name");
            remoteTable.AddColumn("Action");
            remoteTable.AddColumn("Effect");
            remoteTable.AddColumn("Principal");
            remoteTable.AddColumn("Comment");


            var result = await _listAllUseCase.ExecuteAsync();

            foreach (PolicyEntity policy in result)
            {
                remoteTable.AddRow(new Text(policy.Resource.ToString()), new Text(policy.Action.ToString()), new Text(policy.Effect.ToString()), new Text(policy.Principal.ToString()), new Text(policy.Comment.ToString()));
            }

            // Render the table to the console
            AnsiConsole.Write(remoteTable);
            return 0;
        }
        var table = new Table();

        // For now hardcode this as we only have 2 policies       

        table.AddColumn("Policy Name");
        table.AddColumn("Description");

        table.AddRow("[green]AllowCustomLogin[/]", "This allows you to enable client login with CustomId");
        table.AddRow("[green]DenyCustomLogin[/]", "This will block client from login in with CustomId");
        table.AddRow("[green]AllowLinkingCustomId[/]", "This will allow a client linking a CustomId as authentication");
        table.AddRow("[green]DenyLinkingCustomId[/]", "This will block client from linking a CustomId as authentication");


        AnsiConsole.Write(table);

        return 0;
    }
}
