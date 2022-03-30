using PlayFab.AdminModels;

namespace PlayFabBuddy.Infrastructure.Entities.PlayFab.Policy;

public class PolicyEntity
{
    public PolicyEntity(ActionEntity action, EffectEntity effect, ResourceEntity resource, PrincipalEntity principal, string? comment)
    {
        Action = action ?? throw new ArgumentNullException(nameof(action));
        Effect = effect ?? throw new ArgumentNullException(nameof(effect));
        Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        Principal = principal ?? throw new ArgumentNullException(nameof(principal));
        Comment = comment;
    }

    public ActionEntity Action { get; set; }
    public EffectEntity Effect { get; set; }
    public ResourceEntity Resource { get; set; }
    public PrincipalEntity Principal { get; set; }
    public string? Comment { get; set; }

    public PermissionStatement ToPermissionStatement()
    {
        return new PermissionStatement
        {
            Action = Action.ToString(),
            Effect = Effect.Equals(Lib.Entities.Policy.EffectEntity.Type.Allow) ? EffectType.Allow : EffectType.Deny,
            Comment = Comment,
            Principal = Principal.ToString(),
            Resource = Resource.ToString()
        };
    }
}
