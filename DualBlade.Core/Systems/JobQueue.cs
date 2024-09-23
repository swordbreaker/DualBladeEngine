using AutomaticInterface;
using System;
using System.Collections.Generic;

namespace DualBlade.Core.Systems;

[GenerateAutomaticInterface]
public class JobQueue : IJobQueue
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
