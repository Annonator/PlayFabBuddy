using PlayFabBuddy.Lib.UseCases.Policy;
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

        if (settings.PolicyName == "AllowCustomLogin")
        {
            await _allowCustomLoginUseCase.ExecuteAsync();
        }
        else if (settings.PolicyName == "DenyCustomLogin")
        {
            await _denyCustomLoginUseCase.ExecuteAsync();
        }

        return 0;
    }
}
