﻿namespace PlayFabBuddy.Lib.Entities.Accounts;

public class MasterPlayerAccountEntity
{
    public List<TitlePlayerAccountEntity>? PlayerAccounts { get; set; }

    public string? Id { get; set; }
    //Let's work with only a customId for now, re-evaluate when other authentications get important
    public string? CustomId { get; set; }
    
    public string? LastKnownIp { get; set; }
}