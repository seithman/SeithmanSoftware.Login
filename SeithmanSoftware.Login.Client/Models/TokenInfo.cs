using System;
using System.Collections.Generic;
using System.Text;

namespace SeithmanSoftware.Login.Client.Models
{
    public class TokenInfo
    {
        public int OwnerId { get; set; }

        public DateTime Expires { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
