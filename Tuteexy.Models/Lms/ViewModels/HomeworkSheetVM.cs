﻿using System;

namespace Tuteexy.Models.ViewModels
{
    public class HomeworkSheetVM
    {
        public long HomeworkSheetID { get; set; }
        public string ClassRoomName { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public DateTime ScheduleDateTime { get; set; }
        public DateTime DateDue { get; set; }
        public string StudentName { get; set; }
        public string AttachLink1 { get; set; }
        public double HwMarks { get; set; }
        public string HWStatus { get; set; }

    }
}
