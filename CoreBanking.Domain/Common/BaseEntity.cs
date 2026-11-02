namespace CoreBanking.Domain.Common
{
    public class BaseEntity : ISoftDeletable, IEntity, IAudited
    {
        public int Id { get; set; }
        public DateTime InsertedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
