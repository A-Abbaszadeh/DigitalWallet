using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.Site.Models.ViewModels.MoneyTransfer
{
    public class MoneyTransferViewModel
    {
        public long DestinationAccountNumber { get; set; }
        public decimal TransferAmount { get; set; } // مبلغ انتقالی
    }
}
