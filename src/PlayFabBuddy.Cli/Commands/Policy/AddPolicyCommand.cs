using PlayFabBuddy.Lib.UseCases.Policy;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Policy;

internal class AddPolicyCommand : AsyncCommand<AddPolicyCommandSettings>
{
    private readonly AllowCustomLoginUseCase _allowCustomLoginUseCase;
    private readonly DenyCustomLoginUseCase _denyCustomLoginUseCase;
    private readonly DenyLinkingCustomIdUseCase _denyLinkingCustomIdUseCase;
    private readonly AllowLinkingCustomIdUseCase _allowLinkingCustomIdUseCase;

    public AddPolicyCommand(AllowCustomLoginUseCase allowCustomLoginUseCase, DenyCustomLoginUseCase denyCustomLoginUseCase, DenyLinkingCustomIdUseCase denyLinkingCustomIdUseCase, AllowLinkingCustomIdUseCase allowLinkingCustomIdUseCase)
    {
        _allowCustomLoginUseCase = allowCustomLoginUseCase;
        _denyCustomLoginUseCase = denyCustomLoginUseCase;
        _denyLinkingCustomIdUseCase = denyLinkingCustomIdUseCase;
        _allowLinkingCustomIdUseCase = allowLinkingCustomIdUseCase;
    }
    public async override Task<int> ExecuteAsync(CommandContext context, AddPolicyCommandSettings settings)
    {
        await AnsiConsole.Status().StartAsync("Applying policies...", async ctx =>
        {
            ctx.Status("Fetching Policies");
            ctx.Spinner(Spinner.Known.Star);
            ctx.SpinnerStyle(Style.Parse("green"));

            if (settings.PolicyName == "AllowCustomLogin")
            {
                ctx.Status("Applying Policies");
                await _allowCustomLoginUseCase.ExecuteAsync();
                ctx.Status("Waiting for Policies to propagate");
                Thread.Sleep(2000);
            }
            else if (settings.PolicyName == "DenyCustomLogin")
            {
                ctx.Status("Applying Policies");
                await _denyCustomLoginUseCase.ExecuteAsync();
                ctx.Status("Waiting for Policies to propagate");
                Thread.Sleep(2000);
            }
            else if (settings.PolicyName == "DenyLinkingCustomId")
            {
                ctx.Status("Applying Policies");
                await _denyLinkingCustomIdUseCase.ExecuteAsync();
                ctx.Status("Waiting for Policies to propagate");
                Thread.Sleep(2000);
            }
            else if (settings.PolicyName == "AllowLinkingCustomId")
            {
                ctx.Status("Applying Policies");
                await _allowLinkingCustomIdUseCase.ExecuteAsync();
                ctx.Status("Waiting for Policies to propagate");
                Thread.Sleep(2000);
            }
            ctx.Status("Done!");
        });

        AnsiConsole.MarkupLine("[green]Policy \"" + settings.PolicyName + "\" Successfully applied[/]");
        return 0;
    }
}
