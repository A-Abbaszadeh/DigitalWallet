using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Domain.Entities.Users
{
    public class Transaction
    {
        public long Id { get; set; }
        public virtual User User { get; set; }
        public long UserId { get; set; }
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public decimal Debtor { get; set; } // برداشت
        public decimal Creditor { get; set; } // واریز
        public decimal Amount { get; set; } // مانده
    }
}
