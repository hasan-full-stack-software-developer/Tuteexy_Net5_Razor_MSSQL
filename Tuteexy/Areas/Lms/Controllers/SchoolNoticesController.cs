﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tuteexy.DataAccess.Repository.IRepository;
using Tuteexy.Models;
using Tuteexy.Models.ViewModels;
using Tuteexy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Tuteexy.Areas.Lms.Controllers
{
    [Area("Lms")]
    [Authorize(Roles = SD.Role_User)]
    public class SchoolNoticesController : Controller
    {
        private readonly ILogger<SchoolNoticesController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private string _userId;


        public SchoolNoticesController(ILogger<SchoolNoticesController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(long Id)
        {
            var sn = new SchoolNotice();
            sn.SchoolID = Id;
            sn.ScheduleDateTime = DateTime.Now;
            SchoolNoticeVM schoolnoticeVM = new SchoolNoticeVM()
            {
                SchoolNotice = sn,
                ScheduleTime = DateTime.Now.TimeOfDay.ToString()
            };

            //this is for create
            return View("Upsert",schoolnoticeVM);            
        }

        public async Task<IActionResult> Edit(long Id)
        {
            var schoolnotice = await _unitOfWork.SchoolNotice.GetAsync(Id);
            
            //this is for edit
            
            if (schoolnotice == null)
            {
                return NotFound();
            }
            SchoolNoticeVM schoolnoticeVM = new SchoolNoticeVM()
            {
                SchoolNotice = schoolnotice,
                ScheduleTime = schoolnotice.ScheduleDateTime.TimeOfDay.ToString()
            };
            return View("Upsert", schoolnoticeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(SchoolNoticeVM schoolnoticevm)
        {
            if (ModelState.IsValid)
            {
                var workdate = DateTime.Now;
                _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                TimeSpan ts = TimeSpan.Parse(schoolnoticevm.ScheduleTime);
                schoolnoticevm.SchoolNotice.ScheduleDateTime = schoolnoticevm.SchoolNotice.ScheduleDateTime.Add(ts);

                if (schoolnoticevm.SchoolNotice.SchoolNoticeID == 0)
                {
                    schoolnoticevm.SchoolNotice.CreatedBy = _userId;
                    schoolnoticevm.SchoolNotice.CreatedDate = workdate;
                    schoolnoticevm.SchoolNotice.UpdatedBy = _userId;
                    schoolnoticevm.SchoolNotice.UpdatedDate = workdate;

                    await _unitOfWork.SchoolNotice.AddAsync(schoolnoticevm.SchoolNotice);

                }
                else
                {
                    schoolnoticevm.SchoolNotice.UpdatedBy = _userId;
                    schoolnoticevm.SchoolNotice.UpdatedDate = workdate;
                    _unitOfWork.SchoolNotice.Update(schoolnoticevm.SchoolNotice);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(schoolnoticevm.SchoolNotice);
        }


        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var allObj = await _unitOfWork.SchoolNotice.GetAllAsync(includeProperties: "School");
            return Json(new { data = allObj.Select(a => new { id=a.SchoolNoticeID, schoolname=a.School.SchoolName, a.Title, scheduledate = a.ScheduleDateTime.ToString("dd/MMM/yyyy hh:mm tt") }) });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var objFromDb = await _unitOfWork.SchoolNotice.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            await _unitOfWork.SchoolNotice.RemoveEntityAsync(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion

    }
}