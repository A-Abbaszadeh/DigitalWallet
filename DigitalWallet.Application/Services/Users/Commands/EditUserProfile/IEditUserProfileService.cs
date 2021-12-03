using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services.Users.Commands.EditUserProfile
{
    public interface IEditUserProfileService
    {
        ResultDto Execute(RequestEditUserProfileDto request);
    }

    public class EditUserProfileService : IEditUserProfileService
    {
        private readonly IDatabaseContext _context;
        public EditUserProfileService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestEditUserProfileDto request)
        {
            var user = _context.Users.Find(request.UserId);
            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false
                };
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Password = request.Password;
            user.UpdateTime = DateTime.Now;
            _context.Users.Attach(user);
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }

    public class RequestEditUserProfileDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
