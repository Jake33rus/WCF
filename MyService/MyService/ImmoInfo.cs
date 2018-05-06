using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    [DataContract]
    public struct ImmoInfo
    {
        [DataMember]
        public int id;
        [DataMember]
        public string name;

        public ImmoInfo(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
