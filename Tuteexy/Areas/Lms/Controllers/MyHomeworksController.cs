﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tuteexy.DataAccess.Repository.IRepository;
using Tuteexy.Models;
using Tuteexy.Utility;

namespace Tuteexy.Areas.Lms.Controllers
{
    [Area("Lms")]
    [Authorize(Roles = SD.Role_User)]
    public class MyHomeworksController : Controller
    {
        private readonly ILogger<MyHomeworksController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string _userId;

        [BindProperty]
        public HomeworkSheet hwreply { get; set; }

        public MyHomeworksController(ILogger<MyHomeworksController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Homework()
        {
            _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var classroom = await _unitOfWork.ClassRoomStudent.GetFirstOrDefaultAsync(c => c.StudentID == _userId);
            long classroomID = 0;
            if (classroom != null)
            {
                classroomID = classroom.ClassRoomID;
            }
            var allObj = await _unitOfWork.Homework.GetAllAsync(h => h.ClassRoomID == classroomID && h.ScheduleDateTime <= DateTime.Now, h => h.OrderByDescending(p => p.DateDue), includeProperties: "ClassRoom,Teacher");
            // return View(allObj.Select(a => new { Title=a.Title, HomeworkID=a.HomeworkID, TeacherName = a.TeacherName, Subject= a.Subject }));
            return View(allObj.OrderByDescending(a => a.HomeworkID));

        }

        public async Task<IActionResult> HomeworkReply(long Id)
        {

            _userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var hwr = await _unitOfWork.HomeworkSheet.GetFirstOrDefaultAsync(i => i.HomeworkID == Id && i.StudentID == _userId, includeProperties: "Homework");
            if (hwr == null)
            {
                HomeworkSheet hwreply = new HomeworkSheet();
                hwreply.HomeworkID = Id;
                hwreply.StudentID = _userId;
                hwreply.DateSubmitted = DateTime.Now;
                hwreply.Description = "";
                hwreply.AttachLink1 = "";
                hwreply.AttachLink2 = "";
                hwreply.AttachLink3 = "";
                hwreply.AttachLink4 = "";
                hwreply.AttachLink5 = "";
                hwreply.HwMarks = 0;
                hwreply.HWComments = "";
                hwreply.HWStatus = SD.StatusPending;

                await _unitOfWork.HomeworkSheet.AddAsync(hwreply);
                _unitOfWork.Save();

                var tmp = await _unitOfWork.Homework.GetFirstOrDefaultAsync(i => i.HomeworkID == hwreply.HomeworkID);
                hwreply.Homework = tmp;
                return View(hwreply);
            }
            //var allObj = await _unitOfWork.TutorJob.GetAllAsync(c => c.CreatedBy == User.Identity.Name);
            return View(hwr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HomeworkReply()
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var hwr = await _unitOfWork.HomeworkSheet.GetFirstOrDefaultAsync(i => i.HomeworkSheetID == hwreply.HomeworkSheetID);
            if (hwr == null)
            {
                TempData["StatusMessage"] = $"Error : Please check homework";
                return RedirectToAction("Homework");
            }

            hwr.Description = hwreply.Description;
            hwr.DateSubmitted = DateTime.Now;
            hwr.HWStatus = SD.StatusSubmitted;

            var uploads = Path.Combine(webRootPath, @"images\homeworks");

            if (hwr.AttachLink1 != null)
            {
                //this is an edit and we need to remove old image
                var imagePath = Path.Combine(uploads, hwr.AttachLink1.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            if (hwr.AttachLink2 != null)
            {
                //this is an edit and we need to remove old image
                var imagePath = Path.Combine(uploads, hwr.AttachLink2.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            if (hwr.AttachLink3 != null)
            {
                //this is an edit and we need to remove old image
                var imagePath = Path.Combine(uploads, hwr.AttachLink3.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            if (hwr.AttachLink4 != null)
            {
                //this is an edit and we need to remove old image
                var imagePath = Path.Combine(uploads, hwr.AttachLink4.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            if (hwr.AttachLink5 != null)
            {
                //this is an edit and we need to remove old image
                var imagePath = Path.Combine(uploads, hwr.AttachLink5.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            hwr.AttachLink1 = "";
            hwr.AttachLink2 = "";
            hwr.AttachLink3 = "";
            hwr.AttachLink4 = "";
            hwr.AttachLink5 = "";

            var files = HttpContext.Request.Form.Files;
            var i = files.Count;
            var j = 1;
            if (files.Count > 0 && files.Count<=5)
            {

                if (j <= i)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var extenstion = Path.GetExtension(files[0].FileName);
                    var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create);
                    using (filesStreams)
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    hwr.AttachLink1 = fileName + extenstion;
                    j++;
                }

                if (j <= i)
                {

                    string fileName = Guid.NewGuid().ToString();
                    var extenstion = Path.GetExtension(files[1].FileName);
                    var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create);
                    using (filesStreams)
                    {
                        files[1].CopyTo(filesStreams);
                    }
                    hwr.AttachLink2 = fileName + extenstion;
                    j++;
                }

                if (j <= i)
                {

                    string fileName = Guid.NewGuid().ToString();
                    var extenstion = Path.GetExtension(files[2].FileName);
                    var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create);
                    using (filesStreams)
                    {
                        files[2].CopyTo(filesStreams);
                    }
                    hwr.AttachLink3 = fileName + extenstion;
                    j++;
                }

                if (j <= i)
                {

                    string fileName = Guid.NewGuid().ToString();
                    var extenstion = Path.GetExtension(files[3].FileName);
                    var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create);
                    using (filesStreams)
                    {
                        files[3].CopyTo(filesStreams);
                    }
                    hwr.AttachLink4 = fileName + extenstion;
                    j++;
                }

                if (j <= i)
                {

                    string fileName = Guid.NewGuid().ToString();
                    var extenstion = Path.GetExtension(files[4].FileName);
                    var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create);
                    using (filesStreams)
                    {
                        files[4].CopyTo(filesStreams);
                    }
                    hwr.AttachLink5 = fileName + extenstion;
                    j++;
                }

            }


            if (hwr.HomeworkSheetID != 0)
            {
                _unitOfWork.HomeworkSheet.Update(hwr);
            }

            _unitOfWork.Save();
            return RedirectToAction("HomeworkReply", new { Id = hwr.HomeworkID });
        }

    }
}