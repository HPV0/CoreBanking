using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Common
{
    public interface IAudited
    {
        DateTime InsertedAt { get; }
    }
}
