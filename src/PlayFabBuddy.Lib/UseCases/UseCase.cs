namespace PlayFabBuddy.Lib.UseCases;

public abstract class UseCase<T> : IUseCase<T>
{
    public abstract Task<T> ExecuteAsync(IProgress<double>? progress = null);

    async protected Task ReportProgress(List<Task> tasks, IProgress<double>? progress, double completed)
    {
        if (progress != null)
        {
            while (tasks.Any())
            {
                var finishedTask = await Task.WhenAny(tasks);
                tasks.Remove(finishedTask);
                progress.Report(completed);
            }
        }
    }
}
