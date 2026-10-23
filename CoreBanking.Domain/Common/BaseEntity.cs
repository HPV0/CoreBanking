namespace CoreBanking.Domain.Common
{
    public class BaseEntity : ISoftDeletable, IEntity, IAudited
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime InsertedAt { get; private set; }
        public bool IsActive { get; private set; } = true;

        public void SoftDelete()
        {
            IsActive = false;
        }
    }
}
