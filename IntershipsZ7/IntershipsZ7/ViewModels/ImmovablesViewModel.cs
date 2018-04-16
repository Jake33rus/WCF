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

namespace IntershipsZ7.ViewModels
{
    class ImmovablesViewModel:ChangeNotifier
    {
        SaverClient client = new SaverClient("BasicHttpBinding_ISaver");
        ImmoRepos ir = new ImmoRepos();
        ImmoRepos saveIr;
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
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        foreach (var immo in ImmoObsCol)
                          {
                            client.DBSave(immo);
                          }                 
                        MessageBox.Show(client.GetResult(), "IntershipsZ8");
                    }));
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
                        
                        foreach(var immo in ir.Load())
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
            GetTypeList(); 
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
