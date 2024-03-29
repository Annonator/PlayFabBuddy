﻿using PlayFabBuddy.Lib.Aggregate;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IPlayerAccountAdapter
{
    public Task Delete(MasterPlayerAccountAggregate account);
    public Task<MasterPlayerAccountAggregate> LoginWithCustomId(string customId);
    public Task<MasterPlayerAccountAggregate> GetTitleAccountsAndCustomId(MasterPlayerAccountAggregate account);

    public Task<bool> BanPlayerByTitlePlayerAccount(
        List<MasterPlayerAccountAggregate> entityList,
        string reason, uint? banDurationInHours = null, bool banByIp = false);
}