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
        /// проверка были ли изменены поля сущности
        /// </summary>
        static bool isChange = false;
        public bool IsChange
        {
            get { return isChange; }
            set
            {
                isChange = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// команда вызываемая при нажатии на кнопку Save
        /// </summary>
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        SaveChanged();
                    }));
            }
        }
        /// <summary>
        /// откатывает изменения в сущности
        /// </summary>
        private RelayCommand repealCommand;
        public RelayCommand RepealCommand
        {
            get
            {
                return repealCommand ??
                    (repealCommand = new RelayCommand(obj =>
                    {
                        var info = client.CancelEdit();
                        if (!info.IsSuccess)
                        {
                            ChangeableImmo = info.Essence;
                            IsChange = false;
                            MessageBox.Show("Изменения отменены!", "MyApp");
                        }
                        else { MessageBox.Show("Произошла ошибка! -> {0}", info.Message); }
                    }));
            }
        }
        /// <summary>
        /// проверяет были ли изменения в сущности находящейся на редактировании
        /// </summary>
        public void IsSaveChanges()
        {
            if (!isChange)
                return;
            string msg = "Сохранить изменения?";
            MessageBoxResult result = MessageBox.Show(msg, "MyApp", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                SaveChanged();
            }
            if (result == MessageBoxResult.No)
            {
                var info = client.CancelEdit();
                if (!info.IsSuccess)
                {
                    ChangeableImmo = info.Essence;
                    IsChange = false;
                    MessageBox.Show("Изменения отменены!", "MyApp");
                }
                else { MessageBox.Show("Произошла ошибка! -> {0}", info.Message); }
            }

        }
        /// <summary>
        /// изменение доступности для нажатия кнопки "Repeal"
        /// </summary>
        bool isEnabledButton = true;
        public bool IsEnabledButton
        {
            get { return isEnabledButton; }
            set
            {
                isEnabledButton = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// видимость прогресс бара
        /// </summary>
        bool isPBVisible;
        public bool IsPBVisible
        {
            get { return isPBVisible; }
            set
            {
                isPBVisible = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Сохраняет изменения в сущности
        /// </summary>
        private async void SaveChanged()
        {
            string message = null;
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    IsPBVisible = true;
                    var info = client.DBSave();
                    message = info.IsSuccess ? info.Message : "Изменения сохранены";
                });
                await task;
                IsChange = false;
            }
            catch (Exception ex)
            {
                message = string.Format("Произошла ошибка:{0}", ex.Message);

            }
            finally
            {
                IsEnabledButton = true;
                IsPBVisible = false;
                MessageBox.Show(message, "MyApp");
            }
        }
        private Immovables changeableImmo;
        public Immovables ChangeableImmo
        {
            get { return changeableImmo; }
            set
            {
                isProgrammingChange = true;
                changeableImmo = value;
                FieldInit();
                OnPropertyChanged();
                isProgrammingChange = false;
            }
        }
        void FieldInit()
        {
            SelectedType = changeableImmo.Type;
            Name = changeableImmo.Name;
            Footage = changeableImmo.Footage;
            Location = changeableImmo.Location;
            Price = changeableImmo.Price;
            NumberOfRooms = changeableImmo.NumbRooms;
            NumberOfFloors = changeableImmo.NumbFloors;
            ApartmentType = changeableImmo.TypeApart;
            PlotSize = changeableImmo.SizePlot;
            Assigment = changeableImmo.Assigment;
        }
        /// <summary>
        /// проверка на программное заполнения значений полей 
        /// </summary>
        bool isProgrammingChange = false;
        public ImmoEditorViewModel(Immovables immo, ServiceClient client)
        {   
            this.client = client;
            IsSaveChanges();
            ChangeableImmo = immo;
            GetTypeList();
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
                var info = client.SetImmovablesFieldValue(fieldName, val);
                if (info.IsSuccess)
                {
                    MessageBox.Show("Произошла ошибка -> {0}", info.Message);
                    IsChange = false;
                }
                else
                {
                    IsChange = true;
                }
                ChangeableImmo = info.Essence;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Ошибка - {exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


    }
}
}
