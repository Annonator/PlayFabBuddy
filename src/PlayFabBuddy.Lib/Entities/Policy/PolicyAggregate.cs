using System.Collections;

namespace PlayFabBuddy.Lib.Entities.Policy;

public class PolicyAggregate : IEnumerable<PolicyEntity>
{
    private readonly List<PolicyEntity> _policies;

    public PolicyAggregate(List<PolicyEntity> policies)
    {
        _policies = policies;
    }

    public void Add(PolicyEntity entity)
    {
        _policies.Add(entity);
    }

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)_policies).GetEnumerator();
    }

    IEnumerator<PolicyEntity> IEnumerable<PolicyEntity>.GetEnumerator()
    {
        return ((IEnumerable<PolicyEntity>)_policies).GetEnumerator();
    }
}
