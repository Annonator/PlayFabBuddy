using PlayFab;
using PlayFab.AuthenticationModels;
using PlayFabBuddy.Lib.Util.Config;

namespace PlayFabBuddy.Lib.Admin;

public class PlayFabConfig : IConfig
{
    public string TitleId { get; init; }
    public string DeveloperSecret { get; init; }

    public PlayFabConfig(string titleId, string developerSecret)
    {
        TitleId = titleId;
        DeveloperSecret = developerSecret;
    }

    public async void InitAsync()
    {
        PlayFabSettings.staticSettings.TitleId = TitleId;
        PlayFabSettings.staticSettings.DeveloperSecretKey = DeveloperSecret;

        var getTitleEntityTokenRequest = new GetEntityTokenRequest(); //Do not need to set Entity
        var titleEntityResponse = await PlayFabAuthenticationAPI.GetEntityTokenAsync(getTitleEntityTokenRequest);

        //If Login was not Successfull
        if (titleEntityResponse.Result == null)
        {
            throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,
                "Can't authenticate with Developer Secret for titleId: " + TitleId);
        }
    }
}