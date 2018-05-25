using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntershipsZ7.ViewModels
{
    public class FieldViewModel:ChangeNotifier
    {
        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }
        private string caption;
        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                OnPropertyChanged();
            }
        }
        private bool enable;
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;
                OnPropertyChanged();
            }
        }
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }
        private string editorType;

        public FieldViewModel(string value, string caption="", bool enable=true, bool visible=true, string editorType="")
        {
            Value = value;
            Caption = caption;
            Enable = enable;
            Visible = visible;
            EditorType = editorType;
        }

        public string EditorType
        {
            get { return editorType; }
            set
            {
                editorType = value;
                OnPropertyChanged();
            }
        }
    }
}
