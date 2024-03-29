﻿using System.ComponentModel.DataAnnotations;

namespace Tuteexy.Models
{
    public class Attendance : EntryInfo
    {
        [Key]
        public long AttendanceID { get; set; }
        public long StudentID { get; set; }

        //Online/ In Real Life
        [Display(Name = "Attendance Type")]
        [Required]
        [MaxLength(50)]
        public string TypeAttendance { get; set; }
        [Required]
        [Display(Name = "Term 1 - Working Days")]
        public int Term1WorkingDays { get; set; }
        [Required]
        [Display(Name = "Term 1 - Present Days")]
        public int Term1Present { get; set; }
        [Required]
        [Display(Name = "Term 1 - Absent Days")]
        public int Term1Absent { get; set; }
        [Required]
        [Display(Name = "Term 1 - Late Days")]
        public int Term1Late { get; set; }

        // Not [Required] if Term 1 is ongoing
        [Required]
        [Display(Name = "Term 2 - Working Days")]
        public int Term2WorkingDays { get; set; }
        [Required]
        [Display(Name = "Term 2 - Present Days")]
        public int Term2Present { get; set; }
        [Required]
        [Display(Name = "Term 2 - Absent Days")]
        public int Term2Absent { get; set; }
        [Required]
        [Display(Name = "Term 2 - Present Days")]
        public int Term2Late { get; set; }
    }
}
