using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services.Transactions.Queries.GetUserTransactions
{
    public interface IGetUserTransactionsService
    {
        ResultDto<List<ResultGetUserTransactionsServiceDto>> Execute(long UserId); 
    }

    public class GetUserTransactionsService : IGetUserTransactionsService
    {
        private readonly IDatabaseContext _context;
        public GetUserTransactionsService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<ResultGetUserTransactionsServiceDto>> Execute(long UserId)
        {
            var transactions = _context.Transactions
                .Where(t => t.UserId == UserId).ToList()
                .Select(t => new ResultGetUserTransactionsServiceDto
                {
                    InsertTime = t.InsertTime,
                    Description = t.Description,
                    Debtor = t.Debtor,
                    Creditor = t.Creditor,
                    Amount = t.Amount
                }).ToList();
            if (transactions == null)
            {
                return new ResultDto<List<ResultGetUserTransactionsServiceDto>>()
                {
                    Data = null,
                    IsSuccess = false
                };
            }
            return new ResultDto<List<ResultGetUserTransactionsServiceDto>>
            {
                Data = transactions,
                IsSuccess = true
            };
        }
    }
    public class ResultGetUserTransactionsServiceDto
    {
        public DateTime InsertTime { get; set; }
        public string Description { get; set; }
        public decimal Debtor { get; set; } // برداشت
        public decimal Creditor { get; set; } // واریز
        public decimal Amount { get; set; } // مانده
    }
}
