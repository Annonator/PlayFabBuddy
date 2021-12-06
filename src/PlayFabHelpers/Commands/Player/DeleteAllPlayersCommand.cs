using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;

namespace PlayFabBuddy.PlayFabHelpers.Commands.Player
{
    public class DeleteAllPlayersCommand : ICommand
    {
        public async Task ExecuteAsync()
        {
            List<string> masterAccounts = new List<string>();

            masterAccounts.Add("B5D2EE5E21708378");
            masterAccounts.Add("ED843ADFF3C4FBFB");
            masterAccounts.Add("B5D09BB9B6E4AE3C");
            masterAccounts.Add("BF6D423BF54DA3D1");
            masterAccounts.Add("82415338208BE2CD");
            masterAccounts.Add("6626BB6616FFA92E");
            masterAccounts.Add("D8B99EDA0DA8D643");
            masterAccounts.Add("FD341929CED5E207");
            masterAccounts.Add("E22A0342451CC23");
            masterAccounts.Add("6278359942893768");
            masterAccounts.Add("7C6CB406E869267");
            masterAccounts.Add("542788AE6A91514");
            masterAccounts.Add("8FF442AAEBEA92F4");
            masterAccounts.Add("5464A63A4C529658");
            masterAccounts.Add("FC48FB032FFD2086");
            masterAccounts.Add("113A7C6ECCAF67B4");

            foreach (string account in masterAccounts)
            {
                var test = new PlayFab.AdminModels.DeleteMasterPlayerAccountRequest()
                {
                    PlayFabId = account
                };

                await PlayFab.PlayFabAdminAPI.DeleteMasterPlayerAccountAsync(test);
            }

        }
    }
}
