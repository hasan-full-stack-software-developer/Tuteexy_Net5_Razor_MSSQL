﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuteexy.Models
{
    [Table("LmsSchoolTeacher")]
    public class SchoolTeacher

    {
        [Key]
        public long SchoolTeacherID { get; set; }

        public long SchoolID { get; set; }
        [ForeignKey("SchoolID")]
        public virtual School School { get; set; }


        public string TeacherID { get; set; }
        [ForeignKey("TeacherID")]
        public virtual ApplicationUser Teacher { get; set; }

        [MaxLength(50)]
        public string ApprovedBy { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        [Display(Name = "Approved Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime ApprovedDate { get; set; }

        public bool IsApproved { get; set; }
    }
}
