namespace PlayFabBuddy.Lib.Entities.Policy;

public class ResourceEntity
{
    public enum Type
    {
        SelfProfile,
        ProfileStatistics,
        GroupStatistics,
        LoginWithCustomId,
        LinkWithCustomId
    }
    private readonly string SelfProfile = "pfrn:data--*![SELF]/Profile/*";
    private readonly string ProfileStatistics = "pfrn:data--*!*/Profile/Statistics/*";
    private readonly string GroupStatistics = "pfrn:data--group!*/Profile/Statistics/*";
    private readonly string LoginWithCustomId = "pfrn:api--/Client/LoginWithCustomID";
    private readonly string LinkWithCustomId = "pfrn:api--/Client/LinkCustomID";

    private readonly Type _resource;

    public ResourceEntity(Type resource)
    {
        _resource = resource;
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
            default:
                throw new Exception("There needs to be a resource set!");
        }
    }
}
