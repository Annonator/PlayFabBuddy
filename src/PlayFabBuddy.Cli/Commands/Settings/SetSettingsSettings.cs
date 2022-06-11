using System.ComponentModel;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Settings;

public class SetSettingsSettings : SettingsSettings
{
    [Description("The Title ID used by default in any commands a source.")]
    [CommandOption("-o|--overwrite")]
    public bool WantToOverwrite { get; set; } = false;

    [Description("The Title ID used by default in any commands a source.")]
    [CommandOption("-i|--id")]
    public string TitleId { get; set; } = "";

    [Description("The DevSecret for the main Title Id configured")]
    [CommandOption("-s|--secret")]
    public string DevSecret { get; set; } = "";

    [Description("The default path to save the local Master Account config file to be used with PF Buddy")]
    [CommandOption("-m|--masterdefaultpath")]
    public string MasterAccountDefaultSavePath { get; set; } = "MasterAccountOutput.json";
}