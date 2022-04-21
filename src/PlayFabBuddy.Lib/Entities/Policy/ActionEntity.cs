namespace PlayFabBuddy.Lib.Entities.Policy;

public class ActionEntity
{
    public enum Type
    {
        All,
        Read,
        Write
    }
    private readonly string All = "*";
    private readonly string Read = "Read";
    private readonly string Write = "Write";

    private readonly Type _action;

    public ActionEntity(Type action)
    {
        _action = action;
    }

    public ActionEntity(string action)
    {
        if (action == "*")
        {
            _action = Type.All;
        }
        else if (action == "Read")
        {
            _action = Type.Read;
        }
        else if (action == "Write")
        {
            _action = Type.Read;
        }
        else
        {
            throw new ArgumentException("Non Supporte Action Type by Domain Layer");
        }
    }

    public override string ToString()
    {
        switch (_action)
        {
            case Type.Read:
                return Read;
            case Type.Write:
                return Write;
            default: // All is the default behavior
                return All;
        }
    }
}
