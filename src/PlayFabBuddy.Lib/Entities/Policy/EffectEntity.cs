namespace PlayFabBuddy.Lib.Entities.Policy;

public class EffectEntity
{
    public enum Type
    {
        Deny,
        Allow
    }
    private readonly string Deny = "Deny";
    private readonly string Allow = "Allow";
    private readonly Type _effect;


    public EffectEntity(Type effect)
    {
        _effect = effect;
    }

    public override string ToString()
    {
        switch (_effect)
        {
            case Type.Allow:
                return Allow;
            default: //Deny is the default behavior
                return Deny;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is EffectEntity entity &&
               _effect == entity._effect;
    }

    public bool Equals(EffectEntity.Type type)
    {
        return (_effect == type);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_effect);
    }

    public static bool operator ==(EffectEntity? left, EffectEntity? right)
    {
        return EqualityComparer<EffectEntity>.Default.Equals(left, right);
    }

    public static bool operator !=(EffectEntity? left, EffectEntity? right)
    {
        return !(left == right);
    }
}
