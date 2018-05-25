using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    public class ServiceOperationResult
    {
        public ServiceOperationResult(){}
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
