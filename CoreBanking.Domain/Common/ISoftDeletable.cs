namespace CoreBanking.Domain.Common
{
    public interface ISoftDeletable
    {
        public bool IsActive { get; }
        public void SoftDelete();
    }
}

