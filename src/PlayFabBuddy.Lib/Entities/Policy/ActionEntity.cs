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
