using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Filters.IdempotencyFilter
{
    public class IdempotentAttribute : TypeFilterAttribute
    {
        public IdempotentAttribute(int ttlMinutes = 60)
            : base(typeof(IdempotentFilter))
        {
            Arguments = [ttlMinutes];
        }
    }

}
