﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Models
{
    public class User : IdentityUser
    {
        public string DiaChi { get; set; }
    }
}
