using RowanWillis.Common.Application;

namespace MaterialGenerator.Application;

public class TrivialUnitOfWork : IUnitOfWork
{
    public Task CommitChanges(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.CompletedTask;
    }
}