using IntershipsZ7.MyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntershipsZ7.ViewModels
{
    public static class CaсheProperty
    {
        public static PropertyInfo[] ImmoProperty { get; set; }
        public static PropertyInfo[] ImmoEditorProperty { get; set; }
        public static void InitPropInfo()
        {
            ImmoProperty = typeof(Immovables).GetProperties();
            ImmoEditorProperty = typeof(ImmoEditorViewModel).GetProperties();
        }
    }
}
