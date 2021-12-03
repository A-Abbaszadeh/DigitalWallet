using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Common.Dto;
using DigitalWallet.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services.Users.Commands.RegisterUser
{
    public interface IRegisterUserService
    {
        ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request);
    }

    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDatabaseContext _context;
        public RegisterUserService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request)
        {
            try
            {
                User user = new User()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    NationalCode = request.NationalCode,
                    MobileNumber = request.MobileNumber,
                    Password = request.Password
                };

                //Account account = new Account
                //{
                //    UserId = user.Id,
                //    Balance = 500000,
                //    User = user,
                //};

                user.Account = new Account()
                {
                    UserId = user.Id,
                    Balance = 500000,
                    User = user,
                };

                user.Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        UserId = user.Id,
                        Description = "واریز جایزه بابت ثبت نام",
                        Creditor = 500000,
                        Amount = 500000
                    }
                };

                _context.Users.Add(user);

                _context.SaveChanges();

                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = user.Id,
                        AccountNumber = user.Account.AccountNumber
                    },
                    IsSuccess = true
                };
            }
            catch (Exception)
            {

                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = 0,
                        AccountNumber = 0
                    },
                    IsSuccess = false
                };
            }
        }
    }

    public class RequestRegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
    }

    public class ResultRegisterUserDto
    {
        public long UserId { get; set; }
        public long AccountNumber { get; set; }
    }
}
