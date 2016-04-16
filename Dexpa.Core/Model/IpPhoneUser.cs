﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model
{
    public class IpPhoneUser
    {
        [Key]
        public string UserId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Realm { get; set; }
    }
}
