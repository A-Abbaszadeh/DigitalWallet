using System;
using System.Collections.Generic;

namespace DigitalWallet.Domain.Entities.Users
{
    public class Account
    {
        public User User { get; set; }
        public long UserId { get; set; }
        public long AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }
    }
}
