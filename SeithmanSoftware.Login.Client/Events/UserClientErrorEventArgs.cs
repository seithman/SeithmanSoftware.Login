using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeithmanSoftware.Login.Client.Events
{
    public class UserClientErrorEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public object RelatedObject { get; private set; }

        public UserClientErrorEventArgs (string message, object relatedObject = null) : base ()
        {
            Message = message;
            RelatedObject = relatedObject;
        }
    }
}
