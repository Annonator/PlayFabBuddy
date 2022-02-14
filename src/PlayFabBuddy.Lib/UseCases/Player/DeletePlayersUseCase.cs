using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Lib.UseCases.Player;

public class DeletePlayersUseCase : IUseCase<(int removedCount, List<MasterPlayerAccountAggregate> playersInSegment)>
{
    private readonly IRepository<MasterPlayerAccountAggregate> _repository;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public DeletePlayersUseCase(IPlayerAccountAdapter playerAccountAdapter, IRepository<MasterPlayerAccountAggregate> repo)
    {
        _repository = repo;
        _playerAccountAdapter = playerAccountAdapter;
    }

    public async Task<(int removedCount, List<MasterPlayerAccountAggregate> playersInSegment)> ExecuteAsync(IProgress<double>? progress = null)
    {
        var accountList = await _repository.Get();

        var deleteList = new List<Task>();
        foreach (var account in accountList)
        {
            deleteList.Add(_playerAccountAdapter.Delete(account));
        }

        var playersDeleted = 0;
        if (progress != null)
        {
            playersDeleted = await ReportProgress(deleteList, progress, accountList.Count > 0 ? 100 / accountList.Count : 0);
        }
        else
        {
            await Task.WhenAll(deleteList);
            playersDeleted = accountList.Count;
        }

        // We always get all players from the repo and delete them, so we need to clean up the repo after player deletion. In the future we might want to rely on PF responses to make this more reliable
        await _repository.Clear();

        return (playersDeleted, accountList);
    }

    private async Task<int> ReportProgress(List<Task> tasks, IProgress<double> progress, double completed)
    {
        var playersDeleted = 0;
        while (tasks.Any())
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            progress.Report(completed);
            playersDeleted++;
        }

        return playersDeleted;
    }
}