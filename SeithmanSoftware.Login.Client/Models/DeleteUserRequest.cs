using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeithmanSoftware.Login.Client.Models
{
    public class DeleteUserRequest
    {
        public int IdToDelete { get; set; }

        public string AccessToken { get; set; }
    }
}
