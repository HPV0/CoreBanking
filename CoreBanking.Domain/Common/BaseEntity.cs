using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Common
{
    public class BaseEntity : ISoftDeletable, IEntity, IAudited
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime InsertedAt { get; private set; }
        public bool IsActive { get; private set; } = true;
    }
}
