namespace AwesomeGIC.Bank.Domain
{
    public interface IUnitOfWork
    {
        public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
