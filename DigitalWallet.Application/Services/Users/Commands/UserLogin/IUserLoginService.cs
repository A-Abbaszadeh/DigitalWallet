using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services.Users.Commands.UserLogin
{
    public interface IUserLoginService
    {
        ResultDto<ResultUserloginDto> Execute(RequestUserloginDto request);
    }

    public class UserLoginService : IUserLoginService
    {
        private readonly IDatabaseContext _context;
        public UserLoginService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultUserloginDto> Execute(RequestUserloginDto request)
        {
            var user = _context.Users
                .Include(p => p.Account)
                .Where(p => p.MobileNumber.Equals(request.MobileNumber))
                .FirstOrDefault();

            if (user == null)
            {
                return new ResultDto<ResultUserloginDto>()
                {
                    Data = new ResultUserloginDto()
                    {
                        AccountNumber = 0,
                        FirstName = "",
                        UserId = 0
                    },
                    IsSuccess = false
                };
            }

            if (user.Password != request.Password)
            {
                return new ResultDto<ResultUserloginDto>()
                {
                    Data = new ResultUserloginDto()
                    {
                        AccountNumber = 0,
                        FirstName = "",
                        UserId = 0
                    },
                    IsSuccess = false
                };
            }

            return new ResultDto<ResultUserloginDto>()
            {
                Data = new ResultUserloginDto()
                {
                    AccountNumber = user.Account.AccountNumber,
                    FirstName = user.FirstName,
                    UserId = user.Id
                },
                IsSuccess = true
            };
        }
    }
    public class RequestUserloginDto
    {
        public string MobileNumber { get; set; }
        public string Password { get; set; }
    }

    public class ResultUserloginDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public long AccountNumber { get; set; }
    }
}
