using PlayFab;
using PlayFab.AdminModels;
using PlayFabBuddy.Lib.Entities.Policy;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab.Admin;

public class PolicyAdapter : IPolicyAdapter
{
    private readonly PlayFabAdminInstanceAPI _adminApi;

    public PolicyAdapter(PlayFabAdminInstanceAPI adminApi)
    {
        _adminApi = adminApi;
    }

    public async Task AddAsync(PolicyAggregate policyAggregate)
    {
        var statements = ConvertToPermissionStatements(policyAggregate);

        var getRequest = new GetPolicyRequest
        {
            PolicyName = "ApiPolicy"
        };

        var getResult = await _adminApi.GetPolicyAsync(getRequest);
        var existingPolicies = getResult.Result.Statements;

        // Make sure to not duplicate PermissionStatement
        foreach (var policy in existingPolicies)
        {
            foreach (var statement in statements)
            {
                // .Equal() does not work here, so lets check manually
                if (((policy.Action == statement.Action)
                    && (policy.Comment == statement.Comment)
                    && (policy.Effect == statement.Effect)
                    && (policy.Resource == statement.Resource)
                    && (policy.Principal == statement.Principal)
                    && (policy.ApiConditions == statement.ApiConditions)
                    ))
                {
                    statements.Remove(statement);
                    break;
                }
            }
        }

        if (statements.Count > 0)
        {
            var updateRequest = new UpdatePolicyRequest
            {
                PolicyName = "ApiPolicy",
                OverwritePolicy = false,
                Statements = statements,
                PolicyVersion = getResult.Result.PolicyVersion++
            };

            await _adminApi.UpdatePolicyAsync(updateRequest);
        }
    }

    public async Task RemoveAsync(PolicyAggregate policyAggregate)
    {
        var statements = ConvertToPermissionStatements(policyAggregate);

        var getRequest = new GetPolicyRequest
        {
            PolicyName = "ApiPolicy"
        };
        var result = await _adminApi.GetPolicyAsync(getRequest);
        var existingPolicies = result.Result.Statements;
        var initialSize = existingPolicies.Count;

        foreach (var statement in statements)
        {
            foreach (var policy in existingPolicies)
            {
                // .Equal() does not work here, so lets check manually
                if (((policy.Action == statement.Action)
                    && (policy.Comment == statement.Comment)
                    && (policy.Effect == statement.Effect)
                    && (policy.Resource == statement.Resource)
                    && (policy.Principal == statement.Principal)
                    && (policy.ApiConditions == statement.ApiConditions)
                    ))
                {
                    existingPolicies.Remove(policy);
                    break;
                }
            }
        }

        if (initialSize != existingPolicies.Count)
        {
            var updateRequest = new UpdatePolicyRequest
            {
                PolicyName = "ApiPolicy",
                OverwritePolicy = true,
                Statements = existingPolicies,
                PolicyVersion = result.Result.PolicyVersion++
            };

            await _adminApi.UpdatePolicyAsync(updateRequest);
        }
    }

    private List<PermissionStatement> ConvertToPermissionStatements(PolicyAggregate policyAggregate)
    {
        var statements = new List<PermissionStatement>();

        foreach (PolicyEntity policy in policyAggregate)
        {
            statements.Add(new PermissionStatement
            {
                Action = policy.Action.ToString(),
                Comment = policy.Comment != null ? policy.Comment : "",
                Effect = policy.Effect.Equals(EffectEntity.Type.Allow) ? EffectType.Allow : EffectType.Deny,
                Principal = policy.Principal.ToString(),
                Resource = policy.Resource.ToString()
            }); ;
        }

        return statements;
    }
}
