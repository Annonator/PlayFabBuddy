namespace PlayFabBuddy.Infrastructure.Entities.PlayFab.Policy;

public class ActionEntity
{
    private readonly string All = "*";
    private readonly string Read = "Read";
    private readonly string Write = "Write";

    private readonly Lib.Entities.Policy.ActionEntity.Type _action;

    public ActionEntity(Lib.Entities.Policy.ActionEntity.Type action)
    {
        _action = action;
    }

    public ActionEntity(Lib.Entities.Policy.ActionEntity action)
    {
        _action = action.ActionType;
    }

    public override string ToString()
    {
        switch (_action)
        {
            case Lib.Entities.Policy.ActionEntity.Type.Read:
                return Read;
            case Lib.Entities.Policy.ActionEntity.Type.Write:
                return Write;
            default: // All is the default behavior
                return All;
        }
    }
}
