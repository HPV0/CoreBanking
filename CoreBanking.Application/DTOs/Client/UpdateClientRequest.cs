using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Client
{
    public class UpdateClientRequest
    {
        public string PassportNumber { get; set; }
        public string Email { get; set; }
    }
}
