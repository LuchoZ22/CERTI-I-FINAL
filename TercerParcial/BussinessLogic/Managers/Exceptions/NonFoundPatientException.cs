using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPB.BussinessLogic.Managers.Exceptions
{
    public class NonFoundPatientException : Exception
    {
        public NonFoundPatientException() { }
        public NonFoundPatientException(string message) : base(message) { }

        public string LogMessage(string method)
        {
            return "Method: " + method + " Message: " + base.Message;
        }
    }
}
