using PlayFabBuddy.Lib.Entities.Policy;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IPolicyAdapter
{
    public Task AddAsync(PolicyAggregate policyAggregate);
    public Task RemoveAsync(PolicyAggregate policyAggregate);
    public Task<PolicyAggregate> ListAllPoliciesAsync();
}
