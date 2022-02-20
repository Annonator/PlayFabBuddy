using PlayFabBuddy.Lib.Entities.Policy;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.UseCases.Policy;

public class AllowCustomLoginUseCase : UseCase<bool>
{
    private readonly IPolicyAdapter _policyAdapter;

    public AllowCustomLoginUseCase(IPolicyAdapter policyAdapter)
    {
        _policyAdapter = policyAdapter;
    }

    public async override Task<bool> ExecuteAsync(IProgress<double>? progress = null)
    {
        var comment = "Deny all CustomIdLogin";

        //First, lets remove conflicting policies
        var removePolicies = new List<PolicyEntity>
{
            new PolicyEntity(new ActionEntity(ActionEntity.Type.Write), new EffectEntity(EffectEntity.Type.Deny), new ResourceEntity(ResourceEntity.Type.LoginWithCustomId), new PrincipalEntity(), comment)
        };

        var policyAggregate = new PolicyAggregate(removePolicies);
        await _policyAdapter.RemoveAsync(policyAggregate);


        comment = "Allow all CustomIdLogin";
        var addPolicies = new List<PolicyEntity>
{
            new PolicyEntity(new ActionEntity(ActionEntity.Type.Write), new EffectEntity(EffectEntity.Type.Allow), new ResourceEntity(ResourceEntity.Type.LoginWithCustomId), new PrincipalEntity(), comment)
        };

        var addPolicyAggregate = new PolicyAggregate(addPolicies);
        await _policyAdapter.AddAsync(addPolicyAggregate);

        return true;
    }
}
