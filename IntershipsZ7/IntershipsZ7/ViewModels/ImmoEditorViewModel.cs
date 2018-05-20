using IntershipsZ7.MyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
                        CancelEdit();
                    }));
            }
        }
        public void CancelEdit()
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
                CancelEdit();
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
                changeableImmo = value;
                FieldInit();
                OnPropertyChanged();
            }
        }
        void FieldInit()
        {
            try
            {
                isProgrammingChange = true;
                foreach(var immoEditProp in CaсheProperty.ImmoEditorProperty)
                {
                    string propName = immoEditProp.Name;
                    foreach (var immoProp in CaсheProperty.ImmoProperty)
                    {
                        string imPropName = immoProp.Name;
                        if (propName == imPropName)
                        {
                            immoEditProp.SetValue(this, immoProp.GetValue(changeableImmo));
                        }
                    }
                }
                SelectedType = ChangeableImmo.Type;
            }
            finally { isProgrammingChange = false; }
        }
        /// <summary>
        /// проверка на программное заполнения значений полей 
        /// </summary>
        bool isProgrammingChange = false;
        public ImmoEditorViewModel(Immovables immo, ServiceClient client, PropertyInfo[] property )
        {   

            this.client = client;
            ChangeableImmo = immo;
            GetTypeList();
        }
        ServiceClient client;
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
                if (Changed(value)) name = value;
                OnPropertyChanged();
            }
        }

        private double footage;
        public double Footage
        {
            get { return footage; }
            set
            {
               
                if(Changed(value)) footage = value;
                OnPropertyChanged();
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if(Changed(value)) location = value; ;
                OnPropertyChanged();
            }
        }

        private int price;
        public int Price
        {
            get { return price; }
            set
            {
                if(Changed(value)) price = value;
                OnPropertyChanged();
            }
        }

        private int? numbRooms;
        public int? NumbRooms
        {
            get { return numbRooms; }
            set
            {
                if(Changed(value)) numbRooms = value;
                OnPropertyChanged();
            }
        }
    
        private string typeApart;
        public string TypeApart
        {
            get { return typeApart; }
            set
            {
                if(Changed(value)) typeApart = value;
                OnPropertyChanged();
            }
        }

        private int? numbFloors;
        public int? NumbFloors
        {
            get { return numbFloors; }
            set
            {
                if (Changed(value))numbFloors = value;
                OnPropertyChanged();
            }
        }

        private double? sizePlot;
        public double? SizePlot
        {
            get { return sizePlot; }
            set
            {
                if(Changed(value))sizePlot = value;
                OnPropertyChanged();
            }
        }

        private string assigment;   
        public string Assigment
        {
            get { return assigment; }
            set
            {
                if(Changed(value)) assigment = value;
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
                if (Changed(value, "Type")) selectedType = value;
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

        public bool Changed(object val, [CallerMemberName] string fieldName = "")
        {
            if (isProgrammingChange)
                return true;
            try
            {
                var info = client.SetImmovablesFieldValue(fieldName, val);
                if (info.IsSuccess)
                {
                    MessageBox.Show("Произошла ошибка -> {0}", info.Message);
                    IsChange = false;
                    return false;
                }
                else
                {
                    IsChange = true;
                    return true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Ошибка - {exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


    }
}
}
