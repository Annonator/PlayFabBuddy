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

    public Type ResourceType { get; }

    public ResourceEntity(Type resource)
    {
        ResourceType = resource;
    }
}
