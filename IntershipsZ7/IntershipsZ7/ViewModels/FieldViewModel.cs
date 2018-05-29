using IntershipsZ7.Models;
using IntershipsZ7.MyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        ImmoModel immoModel;
        public FieldViewModel(ImmoModel immoModel, string caption)
        {
            this.immoModel = immoModel;
            Caption = caption;
            Enable = true;
            Visible = true;
            VisibleAndEnableInit();
            ValueInit();
            
        }
        static readonly PropertyInfo[] immoProp;
        static FieldViewModel()
        {
            immoProp = typeof(Immovables).GetProperties();
        }
        void ValueInit()
        {
            if (Visible == false)
                return;
            
            foreach(PropertyInfo prop in immoProp)
            {
                if (prop.Name == caption)
                {
                    value = prop.GetValue(immoModel.Immo).ToString();
                }
            }
        }
        void VisibleAndEnableInit()
        {
            switch(immoModel.Immo.Type)
            {
                case 4:
                    if (caption == "Assigment" || caption == "TypeApart")
                    {
                        Visible = false;
                        Enable = false;
                    }
                    break;
                case 3:
                    if(caption=="NumbFloors" || caption == "Assigment" || caption == "SizePlot")
                    {
                        Visible = false;
                        Enable = false;
                    }
                    break;
                case 5:
                    if (caption == "NumbFloors" || caption == "Assigment" || caption == "SizePlot"|| caption == "TypeApart")
                    {
                        Visible = false;
                        Enable = false;
                    }
                    break;
                case 2:
                    if (caption == "NumbFloors" || caption == "TypeApart" || caption == "SizePlot" || caption == "NumbRooms")
                    {
                        Visible = false;
                        Enable = false;
                    }
                    break;
                default:
                    break;
            }
        }
        /*public FieldViewModel(string value, string caption="", bool enable=true, bool visible=true)
        {
            Value = value;
            Caption = caption;
            Enable = enable;
            Visible = visible;
        }*/

    }
}
