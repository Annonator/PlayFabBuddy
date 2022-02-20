namespace PlayFabBuddy.Lib.Entities.Policy;

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
}
