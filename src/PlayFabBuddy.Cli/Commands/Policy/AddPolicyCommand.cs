using PlayFabBuddy.Lib.UseCases.Policy;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Policy;

internal class AddPolicyCommand : AsyncCommand<AddPolicyCommandSettings>
{
    private readonly AllowCustomLoginUseCase _allowCustomLoginUseCase;
    private readonly DenyCustomLoginUseCase _denyCustomLoginUseCase;

    public AddPolicyCommand(AllowCustomLoginUseCase allowCustomLoginUseCase, DenyCustomLoginUseCase denyCustomLoginUseCase)
    {
        _allowCustomLoginUseCase = allowCustomLoginUseCase;
        _denyCustomLoginUseCase = denyCustomLoginUseCase;
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
            ctx.Status("Done!");
        });

        AnsiConsole.MarkupLine("[green]Policy \"" + settings.PolicyName + "\" Successfully applied[/]");
        return 0;
    }
}
