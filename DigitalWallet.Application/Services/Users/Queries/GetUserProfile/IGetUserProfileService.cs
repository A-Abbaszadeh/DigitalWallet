using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services.Users.Queries.GetUserProfile
{
    public interface IGetUserProfileService
    {
        ResultDto<ResultGetUserProfileDto> Execute(long UserId);
    }

    public class GetUserProfileService : IGetUserProfileService
    {
        private readonly IDatabaseContext _context;
        public GetUserProfileService(IDatabaseContext context)
        {
            _context = context;
        }
        ResultDto<ResultGetUserProfileDto> IGetUserProfileService.Execute(long UserId)
        {
            var user = _context.Users.Find(UserId);

            if (user == null)
            {
                return new ResultDto<ResultGetUserProfileDto>()
                {
                    Data = new ResultGetUserProfileDto()
                    {
                        FirstName = "",
                        LastName = "",
                    },
                    IsSuccess = false
                };
            }

            return new ResultDto<ResultGetUserProfileDto>()
            {
                Data = new ResultGetUserProfileDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                },
                IsSuccess = false
            };
        }
    }
    public class ResultGetUserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
