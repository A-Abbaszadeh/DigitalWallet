using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Common.Dto;
using DigitalWallet.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
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
            var sourceAccount = _context.Accounts
                .Where(s => s.AccountNumber == request.SourceAccountNumber).FirstOrDefault();

            var destinationAccount = _context.Accounts.Include(a => a.User).ThenInclude(u => u.Transactions)
                .Where(d => d.AccountNumber == request.DestinationAccountNumber).FirstOrDefault();

            if (sourceAccount == destinationAccount)
            {
                return new ResultDto<ResultMoneyTransfer>
                {
                    Data = new ResultMoneyTransfer
                    {
                        Amount = null,
                        Description = "امکان انتقال وجه به خود غیر مجاز است"
                    },
                    IsSuccess = false
                };
            }
            if (sourceAccount.Balance < request.TransferAmount)
            {
                return new ResultDto<ResultMoneyTransfer>
                {
                    Data = new ResultMoneyTransfer
                    {
                        Amount = null,
                        Description = "مبلغ انتقالی از موجودی حساب مبدا بیشتر است"
                    },
                    IsSuccess = false
                };
            }

            Transaction sourceTransaction = new Transaction()
            {
                UserId = sourceAccount.UserId,
                Debtor = request.TransferAmount,
                Description = $"انتقال وجه از {sourceAccount.AccountNumber} به {destinationAccount.AccountNumber}"
            };

            Transaction destinationTransaction = new Transaction()
            {
                UserId = destinationAccount.UserId,
                Creditor = request.TransferAmount,
                Description = $"انتقال وجه از {sourceAccount.AccountNumber} به {destinationAccount.AccountNumber}"
            };

            try
            {
                using (var databaseTransaction = _context.Database.BeginTransaction())
                {
                    sourceAccount.Balance -= request.TransferAmount;
                    sourceTransaction.Amount = sourceAccount.Balance;

                    destinationAccount.Balance += request.TransferAmount;
                    destinationTransaction.Amount = destinationAccount.Balance;

                    sourceAccount.User.Transactions.Add(sourceTransaction);
                    destinationAccount.User.Transactions.Add(destinationTransaction);

                    _context.SaveChanges();
                    databaseTransaction.Commit();
                }
                return new ResultDto<ResultMoneyTransfer>
                {
                    Data = new ResultMoneyTransfer
                    {
                        Amount = sourceAccount.Balance,
                        Description = sourceTransaction.Description
                    },
                    IsSuccess = true
                };
            }
            catch (DbUpdateException)
            {

                return new ResultDto<ResultMoneyTransfer>
                {
                    Data = new ResultMoneyTransfer
                    {
                        Amount = null,
                        Description = "انتقال وجه انجام نشد"
                    },
                    IsSuccess = false
                };
            }
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
        public decimal? Amount { get; set; }
        public string Description { get; set; }
    }
}
