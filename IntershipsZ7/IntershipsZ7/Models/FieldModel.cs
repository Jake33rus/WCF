using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntershipsZ7.Models
{
    public class FieldModel
    {
        public FieldModel(string value, string caption, bool enable=true, bool visible=true, string editorType="")
        {
            Value = value;
            Caption = caption;
            Enable = enable;
            Visible = visible;
            EditorType = editorType;
        }

        public string Value { get; set; }
        public string Caption { get; set; }
        public bool Enable {get; set;}
        public bool Visible { get; set; }
        public string EditorType { get; set; }

    }
}
