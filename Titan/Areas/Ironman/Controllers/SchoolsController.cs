﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Titan.DataAccess.Repository.IRepository;
using Titan.Utility;

namespace Titan.Areas.Admin.Controllers
{
    [Area("Ironman")]
    [Authorize(Roles = SD.Role_Ironman)]
    public class SchoolsController : Controller
    {
        private readonly ILogger<SchoolsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolsController(ILogger<SchoolsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }



        #region API CALLS



        #endregion
    }
}