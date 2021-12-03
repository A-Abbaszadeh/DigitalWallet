using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Common.Dto;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services.Transactions.Commands.MoneyTransfer
{
    public interface IMoneyTransferService
    {
        ResultDto<ResultMoneyTransfer> Execute(RequestMoneyTransfer request);
    }

    public class MoneyTransferService : IMoneyTransferService
    {
        private readonly IDatabaseContext _context;
        public MoneyTransferService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultMoneyTransfer> Execute(RequestMoneyTransfer request)
        {
            // عملیات با ترنزکشن باید انجام شود
            throw new NotImplementedException();
        }
    }

    public class RequestMoneyTransfer
    {
        public long SourceAccountNumber { get; set; }
        public long DestinationAccountNumber { get; set; }
        public decimal TransferAmount { get; set; } // مبلغ انتقالی
    }
    public class ResultMoneyTransfer
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
