﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tuteexy.Models
{
    [Table("IMPage")]
    public class Page : EntryInfo
    {
        [Key]
        public long PageID { get; set; }

        [Display(Name = "Page Name")]
        [Required]
        [MaxLength(50)]
        public string PageName { get; set; }

        [Display(Name = "Description")]
        [Required]
        [DataType(DataType.Html)]
        public string Description { get; set; }
    }
}