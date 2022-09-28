namespace PlayFabBuddy.UI.Data
{
    public class PlayerData
    {
        public bool? IsBanned { get; internal set; }
        public string? Id { get; internal set; }
        public string? LastKnownIP { get; internal set; }
        public string? TitleId { get; internal set; }
        public string? MasterPlayerAccountId { get; internal set; }
        public string? CustomId { get; internal set; }
    }
}
