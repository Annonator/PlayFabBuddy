using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;

namespace PlayFabBuddy.PlayFabHelpers.Commands.Player
{
    public class LoginPlayerCommand : ICommand
    {
        private readonly TitlePlayerAccountEntity player;

        public LoginPlayerCommand(TitlePlayerAccountEntity player)
        {
            this.player = player;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
