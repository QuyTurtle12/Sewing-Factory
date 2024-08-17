﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingFactory.Services.Dto.UserDto
{
    public class UpdateDto
    {
        
        public string? Name { get; set; }

        public Guid? RoleID { get; set; }

        public Guid? GroupID { get; set; }

        [StringLength(50, MinimumLength = 5)]
        public string? Username { get; set; }

        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        [Range(0, double.MaxValue)]
        public double? Salary { get; set; }
    }
}

