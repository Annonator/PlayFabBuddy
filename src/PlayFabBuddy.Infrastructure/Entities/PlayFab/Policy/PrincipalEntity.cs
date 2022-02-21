namespace PlayFabBuddy.Infrastructure.Entities.PlayFab.Policy;

public class PrincipalEntity
{
    public PrincipalEntity(Lib.Entities.Policy.PrincipalEntity principalEntity)
    {
        //nothing to do here for now
    }

    public override string ToString()
    {
        // Currently only * is supported
        return "*";
    }
}
