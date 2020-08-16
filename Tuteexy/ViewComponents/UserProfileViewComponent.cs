﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tuteexy.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Tuteexy.Models;

namespace Tuteexy.ViewComponents
{
    public class UserProfileViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userFromDb = await _unitOfWork.UserProfile.GetFirstOrDefaultAsync(u => u.UserID == claims.Value,includeProperties:"User");
            if (userFromDb==null)
            {
                UserProfile userprofile = new UserProfile
                {
                    UserID = claims.Value,
                    FatherName = "",
                    MotherName = "",
                    DateOfBirth = DateTime.Now.Date,
                    Gender = "N/A",
                    Religion = "N/A",
                    BloodGroup = "N/A",
                    StreetAddress = "",
                    City = "",
                    State = "",
                    PostalCode = "",
                    Country = "Bangladesh",
                    ECPersonName = "",
                    ECPersonRelation = "",
                    ECPersonPhoneNumber = "",
                    ImageUrl = ""
                };
                await _unitOfWork.UserProfile.AddAsync(userprofile);
                _unitOfWork.Save();

                var newprofile = await _unitOfWork.UserProfile.GetFirstOrDefaultAsync(u => u.UserID == userprofile.UserID, includeProperties: "User");

                return View(newprofile);
            }
            return View(userFromDb);
        }
    }
}