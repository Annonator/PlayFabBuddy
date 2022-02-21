using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Policy;

public class AddPolicyCommandSettings : PolicySettings
{
    [Description("The name of the policy that you want to apply to your Title. A list of available policies can be found with --list")]
    [CommandArgument(0, "<NameOfPolicy>")]
    public string PolicyName { get; set; } = "";

    public override ValidationResult Validate()
    {
        if (PolicyName == "")
        {
            return ValidationResult.Error("Please provide a valid policy for this command!");
        }
        else if (PolicyName != "AllowCustomLogin" && PolicyName != "DenyCustomLogin" && PolicyName != "DenyLinkingCustomId" && PolicyName != "AllowLinkingCustomId") //for now hardcode this
        {
            return ValidationResult.Error("Please use a well konwn policy, you can get a list of implemented policies with policy list");
        }
        else
        {
            return ValidationResult.Success();
        }
    }
}
