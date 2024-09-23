using AutomaticInterface;

namespace DualBlade.Core.Services;

[GenerateAutomaticInterface]
public sealed class JobQueue : IJobQueue
{
    private readonly Queue<Action> _jobs = new();

    public void Enqueue(Action job) =>
        _jobs.Enqueue(job);

    public void Execute()
    {
        while (_jobs.Count != 0)
        {
            var job = _jobs.Dequeue();
            job();
        }
    }
}
