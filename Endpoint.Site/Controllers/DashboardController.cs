using DigitalWallet.Application.Services.Transactions.Queries.GetUserTransactions;
using DigitalWallet.Application.Services.Users.Commands.EditUserProfile;
using DigitalWallet.Application.Services.Users.Queries.GetDashboeardData;
using Endpoint.Site.Models.ViewModels.ProfileVIewModel;
using Endpoint.Site.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.Site.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IGetDashboeardDataService _getDashboeardDataService;
        private readonly IEditUserProfileService _editUserProfileService;
        private readonly IGetUserTransactionsService _getUserTransactionsService;
        public DashboardController(IGetDashboeardDataService getDashboeardDataService
            , IEditUserProfileService editUserProfileService
            , IGetUserTransactionsService getUserTransactionsService)
        {
            _getDashboeardDataService = getDashboeardDataService;
            _editUserProfileService = editUserProfileService;
            _getUserTransactionsService = getUserTransactionsService;
        }
        public IActionResult Profile()
        {
            long userId = ClaimUtility.GetUserId(User).Value;
            return Json(_getDashboeardDataService.Execute(userId));
        }

        [HttpGet]
        public IActionResult Edit()
        {
            long userId = ClaimUtility.GetUserId(User).Value;
            var userProfileData = _getDashboeardDataService.Execute(userId).Data;
            ViewData["userProfileData"] = new EditProfileViewModel
            {
                FirstName = userProfileData.FirstName,
                LastName = userProfileData.LastName,
            };
            return View();
        }

        [HttpPost]
        public IActionResult Edit(EditProfileViewModel request)
        {
            long userId = ClaimUtility.GetUserId(User).Value;
            var editUserProfileResult = _editUserProfileService.Execute(new RequestEditUserProfileDto()
            {
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password
            });
            return Json(editUserProfileResult);
        }

        public IActionResult Transactions()
        {
            long UserId = ClaimUtility.GetUserId(User).Value;
            return Json(_getUserTransactionsService.Execute(UserId));
        }



    }
}
