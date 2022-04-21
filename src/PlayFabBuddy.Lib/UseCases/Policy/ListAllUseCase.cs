using PlayFabBuddy.Lib.Entities.Policy;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.UseCases.Policy;
public class ListAllUseCase : UseCase<PolicyAggregate>
{
    private readonly IPolicyAdapter _policyAdapter;

    public ListAllUseCase(IPolicyAdapter policyAdapter)
    {
        _policyAdapter = policyAdapter;
    }

    public override Task<PolicyAggregate> ExecuteAsync(IProgress<double>? progress = null)
    {
        return _policyAdapter.ListAllPoliciesAsync();
    }
}
