using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Lib.UseCases.Player;

public class LoginPlayerUseCase : IUseCase<bool>
{
    private readonly IRepository<MasterPlayerAccountAggregate> _repository;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public LoginPlayerUseCase(IRepository<MasterPlayerAccountAggregate> repo, IPlayerAccountAdapter playerAccountAdapter)
    {
        _repository = repo;
        _playerAccountAdapter = playerAccountAdapter;
    }

    public async Task<bool> ExecuteAsync(IProgress<double>? progress = null)
    {
        var allMasterPlayerAccounts = await _repository.Get();

        var tasks = new List<Task>();
        foreach (var masterPlayerAccount in allMasterPlayerAccounts)
        {
            if (masterPlayerAccount.MasterPlayerAccount.CustomId != null)
            {
                tasks.Add(_playerAccountAdapter.LoginWithCustomId(masterPlayerAccount.MasterPlayerAccount.CustomId));
            }
        }

        if (progress != null)
        {
            await ReportProgress(tasks, progress, allMasterPlayerAccounts.Count > 0 ? 100 / allMasterPlayerAccounts.Count : 0);
        }

        return true;
    }

    private async Task ReportProgress(List<Task> tasks, IProgress<double> progress, double completed)
    {
        while (tasks.Any())
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            progress.Report(completed);
        }
    }
}
