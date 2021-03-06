﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreProj.Models
{
    public class MEmployee
    {
        [Key]
        public int EmpID { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email"), Display(Name = "Office e-mail")]
        public string Email { get; set; }
        [Required]
        public eDept? Dept { get; set; }
        public string PhotoPath { get; set; }
    }
}
