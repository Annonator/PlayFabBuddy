namespace PlayFabBuddy.Infrastructure.Entities.PlayFab.Policy;

public class ResourceEntity
{
    private readonly string SelfProfile = "pfrn:data--*![SELF]/Profile/*";
    private readonly string ProfileStatistics = "pfrn:data--*!*/Profile/Statistics/*";
    private readonly string GroupStatistics = "pfrn:data--group!*/Profile/Statistics/*";
    private readonly string LoginWithCustomId = "pfrn:api--/Client/LoginWithCustomID";
    private readonly string LinkWithCustomId = "pfrn:api--/Client/LinkCustomID";

    private readonly Lib.Entities.Policy.ResourceEntity.Type _resource;

    public ResourceEntity(Lib.Entities.Policy.ResourceEntity resource)
    {
        _resource = resource.ResourceType;
    }

    public override string ToString()
    {
        switch (_resource)
        {
            case Lib.Entities.Policy.ResourceEntity.Type.SelfProfile:
                return SelfProfile;
            case Lib.Entities.Policy.ResourceEntity.Type.ProfileStatistics:
                return ProfileStatistics;
            case Lib.Entities.Policy.ResourceEntity.Type.GroupStatistics:
                return GroupStatistics;
            case Lib.Entities.Policy.ResourceEntity.Type.LoginWithCustomId:
                return LoginWithCustomId;
            case Lib.Entities.Policy.ResourceEntity.Type.LinkWithCustomId:
                return LinkWithCustomId;
            default:
                throw new Exception("There needs to be a resource set!");
        }
    }
}
