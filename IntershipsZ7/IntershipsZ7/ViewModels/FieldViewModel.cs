using IntershipsZ7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntershipsZ7.ViewModels
{
    public class FieldViewModel:ChangeNotifier
    {
        Func<string,object> getValue;
        Func<string, object, object> setValue;
        Func<bool, bool> getEnabled, getVisible;
        string nameField;
        public object Value
        {
            get
            {
                return getValue(nameField);
            }
            set
            {
                setValue(nameField, value);
                OnPropertyChanged();
            }
        }
        private string caption;
        public string Caption
        {
            get { return nameField; }
            set
            {
                caption = nameField;
                OnPropertyChanged();
            }
        }
        private bool enable;
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = getEnabled();
                OnPropertyChanged();
            }
        }
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = getVisible();
                OnPropertyChanged();
            }
        }
        private string editorType;

       /* public FieldViewModel(T obj, Expression<Func<T,TProp>> property)
        {
            var memberExperession = (MemberExpression) property.Body;
            targetPropertyInfo = (PropertyInfo) memberExperession.Member;
        }*/
        public FieldViewModel(Func<string,object> getValue,Func<string, object, object> setValue, string nameField, Func<bool, bool> getVisible, Func<bool, bool> getEnabled)
        {
            this.getValue = getValue;
            this.nameField = nameField;
            this.setValue = setValue;
            this.getEnabled = getEnabled;
            this.getVisible = getVisible;
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
