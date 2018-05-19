using IntershipsZ7.MyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IntershipsZ7.ViewModels
{
    /// <summary>
    /// Вложенная модель отвечающая за выбранную сущность и ее редактирование 
    /// </summary>
    public class ImmoEditorViewModel:ChangeNotifier
    {
        /// <summary>
        /// проверка на программное заполнения значений полей 
        /// </summary>
        bool isProgrammingChange = false;
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

        /// <summary>
        /// List хранящий соотношение значения id и типов сущностей, служит для заполнения Combobox
        /// </summary>
        public List<RatioTypes> TypesList { get; set; }

        private string name;
        public string Name
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
        public double Footage
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
        public string Location
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
        public int Price
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
        public int? NumberOfRooms
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
        public string ApartmentType
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
        public int? NumberOfFloors
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
        public double? PlotSize
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
        public string Assigment
        {
            get { return assigment; }
            set
            {
                assigment = value;
                Changed("Assigment", assigment);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Значение свойства SelectedValue в комбобоксе показывающем тип сущности
        /// </summary>
        private int selectedType;
        public virtual int SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                Changed("Type", selectedType);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Заполняет лист значениями
        /// </summary>
        public void GetTypeList()
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
