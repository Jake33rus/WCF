using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntershipsZ7.ViewModels
{
    class TypesViewModel:ChangeNotifier
    {
        private int id;
        private string typeName;
        public int Id { get {return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        } 
        public string TypeName
        {
            get { return typeName; }
            set
            {
                typeName = value;
                OnPropertyChanged();
            }
        }
    }
}
