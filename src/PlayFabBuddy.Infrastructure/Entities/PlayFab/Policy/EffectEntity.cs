namespace PlayFabBuddy.Infrastructure.Entities.PlayFab.Policy;

public class EffectEntity
{
    private readonly string Deny = "Deny";
    private readonly string Allow = "Allow";
    private readonly Lib.Entities.Policy.EffectEntity.Type _effect;


    public EffectEntity(Lib.Entities.Policy.EffectEntity.Type effect)
    {
        _effect = effect;
    }

    public EffectEntity(Lib.Entities.Policy.EffectEntity effectEntity)
    {
        _effect = effectEntity.EffectType;
    }

    public override string ToString()
    {
        switch (_effect)
        {
            case Lib.Entities.Policy.EffectEntity.Type.Allow:
                return Allow;
            default: //Deny is the default behavior
                return Deny;
        }
    }

    public bool Equals(Lib.Entities.Policy.EffectEntity.Type type)
    {
        return _effect == type;
    }
}
