namespace PlayFabBuddy.Lib.Entities.Policy;

public class EffectEntity
{
    public enum Type
    {
        Deny,
        Allow
    }
    public Type EffectType { get; }


    public EffectEntity(Type effect)
    {
        EffectType = effect;
    }
}
