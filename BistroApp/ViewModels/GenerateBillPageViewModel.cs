using BistroApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BistroApp.ViewModels
{
    public class GenerateBillPageViewModel : BaseViewModel
    {
        private List<Tables> tables = new List<Tables>() { new Tables { Id = 0, Name = "-- Select Table --" } };
        private ObservableCollection<TableOrderItems> tableOrderItems;
        private Tables selectedTable;
        private ICommand addNewCommand;
        private ICommand clearCommand;
        private double total;

        public GenerateBillPageViewModel()
        {
            SelectedTable = Tables.FirstOrDefault();
            AddButtonEnabled = false;
        }

        public List<Tables> Tables
        {
            get
            {
                return tables;
            }
            set
            {
                SetProperty(ref tables, value);
            }
        }

        public Tables SelectedTable
        {
            get
            {
                return selectedTable;
            }
            set
            {
                SetProperty(ref selectedTable, value);
                AddButtonEnabled = CanAdd();
                ClearButtonEnabled = CanClear();
            }
        }

        public ObservableCollection<TableOrderItems> TableOrderItems
        {
            get
            {
                return tableOrderItems;
            }
            set
            {
                SetProperty(ref tableOrderItems, value);
            }
        }

        public double Total
        {
            get
            {
                return total;
            }
            set
            {
                SetProperty(ref total, value);
            }
        }

        public override void LoadData()
        {
            base.LoadData();
            IEnumerable<Tables> tables = BistroDatabase.Instance.Get<Tables>(DBQuery.GET_TABLES);
            Tables.AddRange(tables);
        }

        protected void Clear()
        {
            ResetFields();
        }

        private void ResetFields()
        {
            SelectedTable = Tables.FirstOrDefault();
            Total = 0;
            AddButtonEnabled = false;
        }

        public bool CanAdd()
        {
            return SelectedTable != null;
        }

        public bool CanClear()
        {
            return SelectedTable != Tables.FirstOrDefault();
        }

        public ICommand AddNewItemCommand
        {
            get
            {
                if (addNewCommand == null)
                {
                    addNewCommand = new RelayCommand<object>(param => GenerateBill(), param => CanAdd());
                }
                return addNewCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new RelayCommand<object>(param => Clear(), param => CanClear());
                }
                return clearCommand;
            }
        }

        public void GenerateBill()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
