﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeithmanSoftware.Login.Client.Models
{
    class LoginRequest
    {
        public string UserNameOrEmail { get; set; }

        public string Password { get; set; }
    }
}
