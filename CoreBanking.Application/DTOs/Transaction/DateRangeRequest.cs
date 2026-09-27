using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Transaction
{
    public class DateRangeRequest
    {
        public DateTime timeFrom { get; set; } = DateTime.MinValue;
        public DateTime timeTo { get; set; } = DateTime.MaxValue;
    }
}
