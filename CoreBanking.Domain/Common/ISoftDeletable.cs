using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Common
{
    public interface ISoftDeletable
    {
        public bool IsActive { get; }
    }
}

