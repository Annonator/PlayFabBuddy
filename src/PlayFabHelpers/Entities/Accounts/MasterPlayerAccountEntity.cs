using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabBuddy.PlayFabHelpers.Entities.Accounts
{
    public class MasterPlayerAccountEntity
    {
        public string Id { get; set; }

        public MasterPlayerAccountEntity(string id)
        {
            Id = id;
        }
    }
}
