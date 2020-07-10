﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Titan.Models
{
    public class userlist
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [MaxLength(50)]
        public string SchoolName { get; set; }
        [MaxLength(50)]
        public string ClassName { get; set; }


    }
}