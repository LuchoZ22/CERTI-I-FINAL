using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPB.BussinessLogic.Managers.Exceptions
{
    public class FailedToGetDataException : Exception
    {
        public FailedToGetDataException() { }

        public FailedToGetDataException(string message) : base(message) { }

        public string LogMessage(string method)
        {
            return "Method: " + method + " Message: " + base.Message;
        }
    }
}
