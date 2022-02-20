using PlayFabBuddy.Lib.Entities.Policy;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.UseCases.Policy;

public class DenyCustomLoginUseCase : UseCase<bool>
{
    private readonly IPolicyAdapter _policyAdapter;

    public DenyCustomLoginUseCase(IPolicyAdapter policyAdapter)
    {
        _policyAdapter = policyAdapter;
    }

    public async override Task<bool> ExecuteAsync(IProgress<double>? progress = null)
    {
        //First remove conflicitng Policies
        var comment = "Allow all CustomIdLogin";

        var removePolicy = new List<PolicyEntity>
{
            new PolicyEntity(new ActionEntity(ActionEntity.Type.All), new EffectEntity(EffectEntity.Type.Allow), new ResourceEntity(ResourceEntity.Type.LoginWithCustomId), new PrincipalEntity(), comment)
        };

        var policyAggregate = new PolicyAggregate(removePolicy);
        await _policyAdapter.RemoveAsync(policyAggregate);

        comment = "Deny all CustomIdLogin";
        var addPolicies = new List<PolicyEntity>
{
            new PolicyEntity(new ActionEntity(ActionEntity.Type.All), new EffectEntity(EffectEntity.Type.Deny), new ResourceEntity(ResourceEntity.Type.LoginWithCustomId), new PrincipalEntity(), comment)
        };
        var addPolicyAggregate = new PolicyAggregate(addPolicies);
        await _policyAdapter.AddAsync(addPolicyAggregate);

        return true;
    }
}
