using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Common
{
    public interface IEntity
    {
        public Guid Id { get; }
    }
}
