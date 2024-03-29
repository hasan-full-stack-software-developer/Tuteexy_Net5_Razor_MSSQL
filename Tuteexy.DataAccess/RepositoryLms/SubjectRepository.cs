﻿using System.Linq;
using Tuteexy.DataAccess.Data;
using Tuteexy.DataAccess.Repository.IRepository;
using Tuteexy.Models;

namespace Tuteexy.DataAccess.Repository
{
    public class SubjectRepository : RepositoryAsync<Subject>, ISubjectRepository
    {
        private readonly ApplicationDbContext _db;

        public SubjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Subject subject)
        {
            var objFromDb = _db.Subject.FirstOrDefault(s => s.SubjectID == subject.SubjectID);
            if (objFromDb != null)
            {
                objFromDb.SubjectName = subject.SubjectName;

            }
        }
    }
}
