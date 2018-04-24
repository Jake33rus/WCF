using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Models;
using CommonLib.Repositories;
using System.Windows;
using System.Collections.ObjectModel;

using System.Reflection;
using System.Data.Entity;
using IntershipsZ7.RemoteService;
using System.Threading;

namespace IntershipsZ7.ViewModels
{
    class ImmovablesViewModel:ChangeNotifier
    {
        public CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        public double MaxPB { get; set; }
        double valuePB;
        public double ValuePB {
        get { return valuePB; }
            set
            {
                valuePB = value;
                OnPropertyChanged();
            }
        }
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
        SaverClient client = new SaverClient("BasicHttpBinding_ISaver");
        ImmoRepos ir = new ImmoRepos();
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
        ImmoRepos saveIr = new ImmoRepos();
        public ObservableCollection<Immovables> ImmoObsCol { get; set; }
        public List<TypesViewModel> TypesList { get; set; }
        private int selectedType;
        private Immovables selectedImmo;
        public virtual int SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                ChangeType();
                OnPropertyChanged();
            }
        }

        public Immovables SelectedImmo
        {
            get { return selectedImmo; }
            set
            {
                selectedImmo = value;
                selectedType = selectedImmo.Type;
                OnPropertyChanged("SelectedType");
                OnPropertyChanged();
                
            }
        }
        private RelayCommand saveCommand;
        public  RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(async obj => 
                    {
                        string message = null;
                        try
                        {
                            var task = Task<string>.Factory.StartNew(Save);
                            IsEnabledButton = false;
                            message = await task;
                        }
                        catch(Exception ex)
                        {
                            message = string.Format("Произошла ошибка:{0}", ex.Message); 
                        }
                        IsEnabledButton = true;
                        IsPBVisible = false;
                        ValuePB = 0;
                        MessageBox.Show(message, "IntershipsZ8");
                    }));
            }
        }
        private string Save()
        {
            string message = null;
            IsPBVisible = true;
            foreach (var immo in ImmoObsCol)
                {
                    var info = client.DBSave(immo);
                    ValuePB++;
                    message = info.IsSuccess ? info.Message : "Изменения сохранены";
                }
            return message;
        }
      private void AutoUpdate(CancellationToken token)
        {        
            ImmoRepos tempIr = new ImmoRepos();
            var temp = tempIr.GetVersion();
            var tempCollection = new ObservableCollection<Immovables>();
            while (!token.IsCancellationRequested)
            {
                var version = tempIr.GetVersion();
                if (!temp.SequenceEqual(version))
                {
                    tempCollection.Clear();
                    foreach (var item in ir.Load())
                    {
                        tempCollection.Add(item);
                    }
                    ImmoObsCol = tempCollection; 
                    temp = version;
                }
                Thread.Sleep(5000);
            }
        }
        private RelayCommand repealCommand;
        public RelayCommand RepealCommand
        {
            get
            {
                return repealCommand ??
                    (repealCommand = new RelayCommand(obj =>
                    {
                        foreach (var immo in ir.Load())
                        { 
                            var temp = saveIr.LoadByID(immo.Id);
                            immo.Type = temp.Type;
                        }
                        ir.SaveChanges();
                        SelectedImmo = selectedImmo;
                        MessageBox.Show("Изменения отменены!", "IntershipsZ8");
                    }));
            }
        }
        public ImmovablesViewModel()
        {
            GetImmovables();
            MaxPB = ImmoObsCol.Count();
            GetTypeList();
            CancellationToken token = cancelTokenSource.Token;
            var hiddenTask = Task.Factory.StartNew(()=>AutoUpdate(token));
        }

        public virtual void GetImmovables()
        { 
            ImmoObsCol = new ObservableCollection<Immovables>();
            foreach (var item in ir.Load()) 
             {
                ImmoObsCol.Add(item);
             }
        }

        public void GetTypeList()
        {
            TypesList = new List<TypesViewModel>();
            TypesList.Add(new TypesViewModel() { Id = 2, TypeName = "NoLivingSpace" });
            TypesList.Add(new TypesViewModel() { Id = 3, TypeName = "Apartments" });
            TypesList.Add(new TypesViewModel() { Id = 4, TypeName = "Houses" });
            TypesList.Add(new TypesViewModel() { Id = 5, TypeName = "LivingSpace" });
        }
       
       public void ChangeType()
        {
            ImmoObsCol.FirstOrDefault(x => x == selectedImmo).Type = SelectedType;          
        }
    }

}
