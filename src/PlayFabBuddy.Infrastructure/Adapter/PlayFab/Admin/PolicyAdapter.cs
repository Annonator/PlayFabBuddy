using PlayFab;
using PlayFab.AdminModels;
using PlayFabBuddy.Infrastructure.Exceptions;
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

    /// <summary>
    /// Adds all policies of the aggregate to your title
    /// </summary>
    /// <param name="policyAggregate"></param>
    /// <returns></returns>
    /// <exception cref="PolicySyntaxException"></exception>
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

            var result = await _adminApi.UpdatePolicyAsync(updateRequest);

            if (result.Error != null)
            {
                throw new PolicySyntaxException(result.Error.ErrorMessage);
            }
        }
    }

    /// <summary>
    /// Removes all Policies defined in the Aggregate from your title
    /// </summary>
    /// <param name="policyAggregate">A Aggregate containign all the Policies that need to be delted</param>
    /// <returns></returns>
    /// <exception cref="PolicySyntaxException"></exception>
    public async Task RemoveAsync(PolicyAggregate policyAggregate)
    {
        var statements = ConvertToPermissionStatements(policyAggregate);

        var getRequest = new GetPolicyRequest
        {
            PolicyName = "ApiPolicy"
        };
        var getResult = await _adminApi.GetPolicyAsync(getRequest);
        var existingPolicies = getResult.Result.Statements;
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
                PolicyVersion = getResult.Result.PolicyVersion++
            };

            var result = await _adminApi.UpdatePolicyAsync(updateRequest);

            if (result.Error != null)
            {
                throw new PolicySyntaxException(result.Error.ErrorMessage);
            }
        }
    }

    /// <summary>
    /// This fetches all the currently set policies in the PlayFab Title
    /// </summary>
    /// <returns>A Task with the PolicyAggregate of all remote policies</returns>
    public async Task<PolicyAggregate> ListAllPoliciesAsync()
    {
        var getRequest = new GetPolicyRequest
        {
            PolicyName = "ApiPolicy"
        };
        var getResult = await _adminApi.GetPolicyAsync(getRequest);
        var existingPolicies = getResult.Result.Statements;

        return ConvertToPolicyAggrgate(existingPolicies);

    }

    /// <summary>
    /// Converts a list of PermissionStatement into a PolicyAggregate of the domain model
    /// </summary>
    /// <param name="statements">The PermissionStatements that needs to be converted</param>
    /// <returns>A PolicyAggregate containing all statements</returns>
    private PolicyAggregate ConvertToPolicyAggrgate(List<PermissionStatement> statements)
    {
        List<PolicyEntity> policyList = new List<PolicyEntity>();

        foreach (var statement in statements)
        {
            ResourceEntity resource;

            switch (statement.Resource)
            {
                case "pfrn:data--*![SELF]/Profile/*": resource = new ResourceEntity(ResourceEntity.Type.SelfProfile); break;
                case "pfrn:data--*!*/Profile/Statistics/*": resource = new ResourceEntity(ResourceEntity.Type.ProfileStatistics); break;
                case "pfrn:data--group!*/Profile/Statistics/*": resource = new ResourceEntity(ResourceEntity.Type.GroupStatistics); break;
                case "pfrn:api--/Client/LoginWithCustomID": resource = new ResourceEntity(ResourceEntity.Type.LoginWithCustomId); break;
                case "pfrn:api--/Client/LinkCustomID": resource = new ResourceEntity(ResourceEntity.Type.LinkWithCustomId); break;
                default:
                    resource = new ResourceEntity(ResourceEntity.Type.Unknown);
                    resource.SetUnkownTypeDescription(statement.Resource);
                    break;
            }

            ActionEntity action;

            switch (statement.Action)
            {
                case "Read": action = new ActionEntity(ActionEntity.Type.Read); break;
                case "Write": action = new ActionEntity(ActionEntity.Type.Write); break;
                default: action = new ActionEntity(ActionEntity.Type.All); break;
            }

            policyList.Add(new PolicyEntity(
                action,
                new EffectEntity((statement.Effect.Equals(EffectType.Allow) ? EffectEntity.Type.Allow : EffectEntity.Type.Deny)),
                resource,
                new PrincipalEntity(),
                statement.Comment));
        }

        return new PolicyAggregate(policyList);
    }

    /// <summary>
    /// Convert a Domain Policy Aggregate into a PlayFab Permission Statement
    /// </summary>
    /// <param name="policyAggregate">The aggregate that needs to be converted</param>
    /// <returns>A list of PersmissiontStatements of all Policies in the PolicyAggregate</returns>
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
