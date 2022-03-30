namespace PlayFabBuddy.Lib.Entities.Policy;

public class ActionEntity
{
    public enum Type
    {
        All,
        Read,
        Write
    }
    public Type ActionType { get; }

    public ActionEntity(Type action)
    {
        ActionType = action;
    }
}
