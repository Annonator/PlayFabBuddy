namespace PlayFabBuddy.Lib.Entities.Policy;

public class ResourceEntity
{
    public enum Type
    {
        SelfProfile,
        ProfileStatistics,
        GroupStatistics,
        LoginWithCustomId,
        LinkWithCustomId,
        Unknown
    }
    private readonly string SelfProfile = "pfrn:data--*![SELF]/Profile/*";
    private readonly string ProfileStatistics = "pfrn:data--*!*/Profile/Statistics/*";
    private readonly string GroupStatistics = "pfrn:data--group!*/Profile/Statistics/*";
    private readonly string LoginWithCustomId = "pfrn:api--/Client/LoginWithCustomID";
    private readonly string LinkWithCustomId = "pfrn:api--/Client/LinkCustomID";
    private string UnknownDescription = "Unknown";

    private readonly Type _resource;

    public ResourceEntity(Type resource)
    {
        _resource = resource;
    }

    public void SetUnkownTypeDescription(string description)
    {
        if (_resource != Type.Unknown)
        {
            throw new NotImplementedException("You can't assigne a description to Unkown Types");
        }

        UnknownDescription = description;
    }

    public override string ToString()
    {
        switch (_resource)
        {
            case Type.SelfProfile:
                return SelfProfile;
            case Type.ProfileStatistics:
                return ProfileStatistics;
            case Type.GroupStatistics:
                return GroupStatistics;
            case Type.LoginWithCustomId:
                return LoginWithCustomId;
            case Type.LinkWithCustomId:
                return LinkWithCustomId;
            case Type.Unknown:
                return UnknownDescription;
            default:
                throw new Exception("There needs to be a resource set!");
        }
    }
}
