namespace CoreBanking.Domain.Common
{
    public interface IAudited
    {
        DateTime InsertedAt { get; }
    }
}
