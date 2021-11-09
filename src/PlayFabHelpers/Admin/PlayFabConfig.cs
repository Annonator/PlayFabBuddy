using PlayFab;
using PlayFab.AuthenticationModels;

namespace PlayFabBuddy.PlayFabHelpers.Admin
{
    public class PlayFabConfig
    {
        public string TitleId { get; init; }
        public string DeveloperSecret { get; init; }

        public async void InitAsync()
        {
            PlayFabSettings.staticSettings.TitleId = TitleId;
            PlayFabSettings.staticSettings.DeveloperSecretKey = DeveloperSecret;

            var getTitleEntityTokenRequest = new GetEntityTokenRequest(); //Do not need to set Entity
            var titleEntityResponse = await PlayFabAuthenticationAPI.GetEntityTokenAsync(getTitleEntityTokenRequest);

            //If Login was not Successfull
            if (titleEntityResponse.Result == null)
            {
                throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn, "Can't authenticate with Developer Secret for titleId: " + this.TitleId);
            }

        }

    }
}
