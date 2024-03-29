﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tuteexy.DataAccess.Repository.IRepository;
using Tuteexy.Models.ViewModels;
using Tuteexy.Utility;

namespace Tuteexy.Areas.Lms.Controllers
{
    [Area("Lms")]
    [Authorize(Roles = SD.Role_User)]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private string _userId;

        public DashboardController(ILogger<DashboardController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            long classrooomid = 0;
            long schoolid = 0;
            var _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var classroomStudents = await _unitOfWork.ClassRoomStudent.GetFirstOrDefaultAsync(c => c.StudentID == _userId);
            if (classroomStudents != null)
            {
                classrooomid = classroomStudents.ClassRoomID;
                var classRoom = await _unitOfWork.ClassRoom.GetFirstOrDefaultAsync(c => c.ClassRoomID == classroomStudents.ClassRoomID);
                schoolid = classRoom.SchoolID;
            }
            var classwork = await _unitOfWork.Classwork.GetAllAsync(h => h.ClassRoomID == classrooomid && h.TimeStart <= DateTime.Now && h.TimeStart.Date == DateTime.Now.Date, h => h.OrderByDescending(p => p.TimeStart), includeProperties: "ClassRoom,Teacher");
            var homework = await _unitOfWork.Homework.GetAllAsync(h => h.ClassRoomID == classrooomid && h.ScheduleDateTime <= DateTime.Now && h.ScheduleDateTime.Date == DateTime.Now.Date, h => h.OrderByDescending(p => p.DateDue), includeProperties: "ClassRoom,Teacher");
            var schoolnotice = await _unitOfWork.SchoolNotice.GetAllAsync(h => h.SchoolID == schoolid && h.ScheduleDateTime <= DateTime.Now && h.isPined == true, h => h.OrderByDescending(p => p.ScheduleDateTime), includeProperties: "School");
            var classroomnotice = await _unitOfWork.ClassRoomNotice.GetAllAsync(h => h.ClassRoomID == classrooomid && h.ScheduleDateTime <= DateTime.Now && h.ScheduleDateTime.Date == DateTime.Now.Date, h => h.OrderByDescending(p => p.ScheduleDateTime), includeProperties: "ClassRoom");

            var userFromDb = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == _userId);

            ChatVM chatVM = new ChatVM
            {
                UserName = userFromDb.FullName,
                GroupName = "GrobalSchool"
            };

            SchoolHomeVM schoolhome = new SchoolHomeVM()
            {
                ChatVM=chatVM,
                Classwork = classwork,
                Homework = homework,
                SchoolNotice = schoolnotice,
                ClassRoomNotice = classroomnotice
            };


            return View(schoolhome);
        }

        public IActionResult Classroutines()
        {
            return View();
        }

        public async Task<IActionResult> Holidays()
        {
            _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value; // "182596ba-2fcc-4db7-8053-395e1af1a276";//
            var classroom = await _unitOfWork.ClassRoomStudent.GetFirstOrDefaultAsync(c => c.StudentID == _userId, includeProperties: "ClassRoom");
            long schoolID = 0;
            if (classroom != null)
            {
                schoolID = classroom.ClassRoom.SchoolID;
            }
            var allObj = await _unitOfWork.Holiday.GetAllAsync(c => c.School.SchoolID == schoolID, includeProperties: "School");

            return View(allObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClassRoutine()
        {
            _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value; // "182596ba-2fcc-4db7-8053-395e1af1a276";//
            var classroom = await _unitOfWork.ClassRoomStudent.GetFirstOrDefaultAsync(c => c.StudentID == _userId);
            long classroomID = 0;
            if (classroom != null)
            {
                classroomID = classroom.ClassRoomID;
            }
            var allObj = await _unitOfWork.ClassRoutine.GetAllAsync(t => t.ClassRoomID == classroomID, includeProperties: "ClassRoom");

            return Json(new
            {
                data = allObj.Select(o => new
                {
                    id = o.ClassRoutineID,
                    classname = o.ClassRoom.ClassRoomName,
                    day = o.DayName,
                    p1 = o.Period1 + " " + o.PeriodTime1.ToString("hh:mm tt"),
                    p2 = o.Period2 + " " + o.PeriodTime2.ToString("hh:mm tt"),
                    p3 = o.Period3 + " " + o.PeriodTime3.ToString("hh:mm tt"),
                    p4 = o.Period4 + " " + o.PeriodTime4.ToString("hh:mm tt"),
                    p5 = o.Period5 + " " + o.PeriodTime5.ToString("hh:mm tt"),
                    p6 = o.Period6 + " " + o.PeriodTime6.ToString("hh:mm tt"),
                    p7 = o.Period7 + " " + o.PeriodTime7.ToString("hh:mm tt"),
                    p8 = o.Period8 + " " + o.PeriodTime8.ToString("hh:mm tt"),
                    p9 = o.Period9 + " " + o.PeriodTime9.ToString("hh:mm tt"),
                    p10 = o.Period10 + " " + o.PeriodTime10.ToString("hh:mm tt")
                })
            });

        }





        [HttpGet]
        public async Task<IActionResult> ClassNotices()
        {
            _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var classroom = await _unitOfWork.ClassRoomStudent.GetFirstOrDefaultAsync(c => c.StudentID == _userId);
            long classroomID = 0;
            if (classroom != null)
            {
                classroomID = classroom.ClassRoomID;
            }
            var allObj = await _unitOfWork.ClassRoomNotice.GetAllAsync(h => h.ClassRoomID == classroomID, h => h.OrderByDescending(p => p.ScheduleDateTime), includeProperties: "ClassRoom");
            return View(allObj);

        }

        [HttpGet]
        public async Task<IActionResult> SchoolNotices()
        {
            _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var classroom = await _unitOfWork.ClassRoomStudent.GetFirstOrDefaultAsync(c => c.StudentID == _userId, includeProperties: "ClassRoom");
            long schoolID = 0;
            if (classroom != null)
            {
                schoolID = classroom.ClassRoom.SchoolID;
            }
            var allObj = await _unitOfWork.SchoolNotice.GetAllAsync(h => h.SchoolID == schoolID, h => h.OrderByDescending(p => p.ScheduleDateTime), includeProperties: "School");
            return View(allObj);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllHoliday()
        {
            //_userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var classroom = await _unitOfWork.ClassRoomStudent.GetFirstOrDefaultAsync(includeProperties: "ClassRoom");
            long schoolID = 0;
            if (classroom != null)
            {
                schoolID = classroom.ClassRoom.SchoolID;
            }
            var allObj = await _unitOfWork.Holiday.GetAllAsync(c => c.School.SchoolID == schoolID, includeProperties: "School");
            return Json(new { data = allObj.Select(a => new { id = a.HolidayID, schoolname = a.School.SchoolName, datestart = a.DateStart.ToString("dd/MMM/yyyy"), dateend = a.DateEnd.ToString("dd/MMM/yyyy"), holidayname = a.HolidayName, duration = a.Duration }) });

        }

    }
}