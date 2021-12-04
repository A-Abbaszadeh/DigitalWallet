using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services.Users.Queries.GetDashboeardData
{
    public interface IGetDashboeardDataService
    {
        ResultDto<ResultGetDashboeardDataDto> Execute(long UserId);
    }

    public class GetDashboeardDataService : IGetDashboeardDataService
    {
        private readonly IDatabaseContext _context;
        public GetDashboeardDataService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultGetDashboeardDataDto> Execute(long UserId)
        {
            var user = _context.Users
                .Include(u => u.Account)
                .Include(u => u.Transactions)
                .Where(u => u.Id.Equals(UserId))
                .FirstOrDefault();

            if (user == null)
            {
                return new ResultDto<ResultGetDashboeardDataDto>()
                {
                    Data = new ResultGetDashboeardDataDto()
                    {
                        FirstName = "",
                        LastName = "",
                        AccountNumber = 0,
                        Balance = 0,
                        TotalTransactions =0
                    },
                    IsSuccess = false
                };
            }

            return new ResultDto<ResultGetDashboeardDataDto>()
            {
                Data = new ResultGetDashboeardDataDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    AccountNumber = user.Account.AccountNumber,
                    Balance = user.Account.Balance,
                    TotalTransactions = user.Transactions.Count
                },
                IsSuccess = true
            };
        }
    }

    public class ResultGetDashboeardDataDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int TotalTransactions { get; set; }
    }
}
