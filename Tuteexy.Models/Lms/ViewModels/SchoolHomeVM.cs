﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Tuteexy.Models.ViewModels
{
    public class SchoolHomeVM
    {
        public IEnumerable<Homework> Homework { get; set; }
        public IEnumerable<Classwork> Classwork { get; set; }
        public IEnumerable<SchoolNotice> SchoolNotice { get; set; }
        public IEnumerable<ClassRoomNotice> ClassRoomNotice { get; set; }
    }
}