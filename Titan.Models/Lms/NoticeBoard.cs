﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Titan.Models
{
    public class NoticeBoard
    {
        public long NoticeBoardID { get; set; }
        public long SchoolID { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "Notice Title")]
        public string NoticeTitle { get; set; }

        [DataType(DataType.Html)]
        [Required]
        [MaxLength(4096)]
        [Display(Name = "Notice Description:")]
        public string NoticeBody { get; set; }
    }
}
