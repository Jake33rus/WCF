using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    public class EssenceSOR<T>:ServiceOperationResult
    {
        public T Essence { get; set; }
    }
}
