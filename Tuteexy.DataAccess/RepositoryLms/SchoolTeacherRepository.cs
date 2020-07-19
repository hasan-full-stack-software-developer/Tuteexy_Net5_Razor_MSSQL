﻿using Tuteexy.DataAccess.Data;
using Tuteexy.DataAccess.Repository.IRepository;
using Tuteexy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuteexy.DataAccess.Repository
{
    public class SchoolTeacherRepository : RepositoryAsync<SchoolTeacher>, ISchoolTeacherRepository
    {
        private readonly ApplicationDbContext _db;

        public SchoolTeacherRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SchoolTeacher schoolteacher)
        {
            var objFromDb = _db.SchoolTeacher.FirstOrDefault(s => s.SchoolTeacherID == schoolteacher.SchoolTeacherID);
            if (objFromDb != null)
            {
                objFromDb.ApprovedBy = schoolteacher.ApprovedBy;
                objFromDb.ApprovedDate = schoolteacher.ApprovedDate;
                objFromDb.IsApproved = schoolteacher.IsApproved;  
            }
        }
    }
}