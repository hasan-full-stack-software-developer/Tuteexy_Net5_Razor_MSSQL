﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuteexy.Models
{
    [Table("LmsSchool")]
    public class School : EntryInfo
    {
        [Key]
        public long SchoolID { get; set; }

        [Display(Name = "School Name")]
        [Required]
        [MaxLength(100)]
        public string SchoolName { get; set; }

        [Display(Name = "Street Address")]
        [MaxLength(100)]
        public string StreetAddress { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string State { get; set; }

        [Display(Name = "Postal Code")]
        [MaxLength(100)]
        public string PostalCode { get; set; }

        [Display(Name = "Country")]
        [MaxLength(100)]
        public string Country { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        public bool IsAuthorizedSchool { get; set; }

        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }

        [Display(Name = "Short Name")]
        [Required]
        [MaxLength(10)]
        public string ShortName { get; set; }

    }
}
