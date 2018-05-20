using CommonLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    public static class CacheImmoProperty
    {
        public static PropertyInfo[] ImmoProperty { get; set; } 
        public static void PropInit()
        {
            ImmoProperty = typeof(Immovables).GetProperties();
        }

    }
}
