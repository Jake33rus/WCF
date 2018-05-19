using IntershipsZ7.MyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IntershipsZ7.ViewModels
{
    public class ImmoEditorViewModel:ChangeNotifier
    {
        bool isProgrammingChange = false; // проверка на программное заполнения значений полей 
        ImmovablesViewModel immoVM;
        public ImmoEditorViewModel(Immovables immo, ServiceClient client, ImmovablesViewModel obj)
        {
            isProgrammingChange = true;
            GetTypeList();
            SelectedType = immo.Type;
            Name = immo.Name;
            Footage = immo.Footage;
            Location = immo.Location;
            Price = immo.Price;
            NumberOfRooms = immo.NumbRooms;
            NumberOfFloors = immo.NumbFloors;
            ApartmentType = immo.TypeApart;
            PlotSize = immo.SizePlot;
            Assigment = immo.Assigment;
            isProgrammingChange = false;
            this.client = client;
            immoVM = obj;
        }
        ServiceClient client;
        public ImmoEditorViewModel()
        {
        }

        public List<RatioTypes> TypesList { get; set; } // List хранящий соотношение значения id и типов сущностей, служит для заполнения Combobox

        private string name;
        public string Name //Значение свойства Text в текстбоксе Name
        {
            get { return name; }
            set
            {
                name = value;
                Changed("Name", name);
                OnPropertyChanged();
            }
        }

        private double footage;
        public double Footage //Значение свойства Text в текстбоксе Footage
        {
            get { return footage; }
            set
            {
                footage = value;
                Changed("Footage", footage);
                OnPropertyChanged();
            }
        }

        private string location;
        public string Location //Значение свойства Text в текстбоксе Location
        {
            get { return location; }
            set
            {
                location = value;
                Changed("Location", location);
                OnPropertyChanged();
            }
        }

        private int price;
        public int Price //Значение свойства Text в текстбоксе Price
        {
            get { return price; }
            set
            {
                price = value;
                Changed("Price", price);
                OnPropertyChanged();
            }
        }

        private int? numberOfRooms;
        public int? NumberOfRooms //Значение свойства Text в текстбоксе NumbRooms
        {
            get { return numberOfRooms; }
            set
            {
                numberOfRooms = value;
                Changed("NumbRooms", numberOfRooms);
                OnPropertyChanged();
            }
        }
    
        private string apartmentType;
        public string ApartmentType //Значение свойства Text в текстбоксе TypeApart
        {
            get { return apartmentType; }
            set
            {
                apartmentType = value;
                Changed("TypeApart", apartmentType);
                OnPropertyChanged();
            }
        }

        private int? numberOfFloors;
        public int? NumberOfFloors //Значение свойства Text в текстбоксе NumbFloors
        {
            get { return numberOfFloors; }
            set
            {
                numberOfFloors = value;
                Changed("NumbFloors", numberOfFloors);
                OnPropertyChanged();
            }
        }

        private double? plotSize;
        public double? PlotSize //Значение свойства Text в текстбоксе PlotSize
        {
            get { return plotSize; }
            set
            {
                plotSize = value;
                Changed("SizePlot", plotSize);
                OnPropertyChanged();
            }
        }

        private string assigment;   
        public string Assigment //Значение свойства Text в текстбоксе Assigment
        {
            get { return assigment; }
            set
            {
                assigment = value;
                Changed("Assigment", assigment);
                OnPropertyChanged();
            }
        }

        private int selectedType;
        public virtual int SelectedType //Значение свойства SelectedValue в комбобоксе показывающем тип сущности
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                Changed("Type", selectedType);
                OnPropertyChanged();
            }
        }

        public void GetTypeList() // заполняет лист значениями
        {
            TypesList = new List<RatioTypes>();
            TypesList.Add(new RatioTypes() { Id = 2, TypeName = "NoLivingSpace" });
            TypesList.Add(new RatioTypes() { Id = 3, TypeName = "Apartments" });
            TypesList.Add(new RatioTypes() { Id = 4, TypeName = "Houses" });
            TypesList.Add(new RatioTypes() { Id = 5, TypeName = "LivingSpace" });
        }

        public void Changed(string fieldName, object val)
        {
            if (isProgrammingChange)
                return;
            try
            {
                client.SetImmovablesFieldValue(fieldName, val);
                immoVM.IsChange = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Ошибка - {exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


    }
}
}
