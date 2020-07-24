using BistroApp.Enums;
using BistroApp.Extensions;
using BistroApp.Models;
using Dapper;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BistroApp.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        private ObservableCollection<CategoryOrderItem> categoryOrderItems = new ObservableCollection<CategoryOrderItem>();
        private List<SubCategory> subCategories = new List<SubCategory>() { new SubCategory { Id = 0, Name = "-- Select Category --" } };
        private string orderItemName;
        private double price;
        private string description;
        private SubCategory selectedSubCategory;
        private RelayCommand<object> addNewCommand;
        private RelayCommand<object> clearCommand;
        private RelayCommand<object> updateCommand;
        private RelayCommand<object> deleteCommand;
        private int orderItemId;
        private int sCOId;

        public MenuPageViewModel()
        {
            NotificationManager = new NotificationManager();
            SelectedSubCategory = SubCategories.FirstOrDefault();
        }

        public ObservableCollection<CategoryOrderItem> CategoryOrderItems
        {
            get
            {
                return categoryOrderItems;
            }
            set
            {
                SetProperty(ref categoryOrderItems, value);
            }
        }

        public List<SubCategory> SubCategories
        {
            get
            {
                return subCategories;
            }
            set
            {
                SetProperty(ref subCategories, value);
            }
        }

        public int SCOId
        {
            get
            {
                return sCOId;
            }
            set
            {
                SetProperty(ref sCOId, value);
            }
        }

        public int OrderItemId
        {
            get
            {
                return orderItemId;
            }
            set
            {
                SetProperty(ref orderItemId, value);
            }
        }

        public string OrderItemName
        {
            get
            {
                return orderItemName;
            }
            set
            {
                SetProperty(ref orderItemName, value);
                AddButtonEnabled = CanAdd();
                ClearButtonEnabled = CanClear();
            }
        }

        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                SetProperty(ref price, value);
                AddButtonEnabled = CanAdd();
                ClearButtonEnabled = CanClear();
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                SetProperty(ref description, value);
                ClearButtonEnabled = CanClear();
            }
        }

        public SubCategory SelectedSubCategory
        {
            get
            {
                return selectedSubCategory;
            }
            set
            {
                SetProperty(ref selectedSubCategory, value);
                AddButtonEnabled = CanAdd();
                ClearButtonEnabled = CanClear();
            }
        }

        public ICommand AddNewItemCommand
        {
            get
            {
                if (addNewCommand == null)
                {
                    addNewCommand = new RelayCommand<object>(param => AddNew(), param => CanAdd());
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

        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new RelayCommand<object>(param => Update(), param => CanUpdate());
                }
                return updateCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand<object>(param => Delete(), param => CanDelete());
                }
                return deleteCommand;
            }
        }

        public int SubCategoryOrderItemId { get; set; }

        public bool CanDelete()
        {
            return IsEditMode && OrderItemId > 0 && SCOId > 0;
        }

        protected void Delete()
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@SCOId", SCOId);
                parameter.Add("@SubCategoryId", SelectedSubCategory.Id);
                parameter.Add("@OrderItemId", OrderItemId);
                bool success = BistroDatabase.Instance.ExecuteQuery(DBQuery.DELETE_SUB_CATEGORY_ORDER_ITEM, parameter);
                AfterExecuteAction(success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void AfterExecuteAction(bool success)
        {
            if (success)
            {
                LoadData();
                ResetFields();
                Notify("Info", "Action successfully executed!", NotificationType.Success);
            }
            else
            {
                Notify("Error", "Unable to execute action!", NotificationType.Error);
            }
        }

        public bool CanUpdate()
        {
            return IsEditMode && OrderItemId > 0;
        }

        private void Update()
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@OrderItemId", OrderItemId);
                parameters.Add("@Name", OrderItemName);
                parameters.Add("@Price", Price);
                parameters.Add("@Description", Description);
                parameters.Add("@SubCategoryId", SelectedSubCategory.Id);
                parameters.Add("@SubCategoryOrderItemId", SubCategoryOrderItemId);

                bool success = BistroDatabase.Instance.OrderItemUpdate(DBQuery.UPDATE_ORDER_ITEM, DBQuery.UPDATE_SUB_CATEGORY_ORDER_ITEM, parameters);
                AfterExecuteAction(success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected void Clear()
        {
            ResetFields();
        }

        public void AddNew()
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", OrderItemName);
                parameters.Add("@Price", Price);
                parameters.Add("@Description", Description);
                parameters.Add("@SubCategoryId", SelectedSubCategory.Id);
                bool success = BistroDatabase.Instance.OrderItemInsert(DBQuery.INSERT_ORDER_ITEM, DBQuery.INSERT_SUB_CATEGORY_ORDER_ITEM, parameters);
                AfterExecuteAction(success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ResetFields()
        {
            OrderItemName = string.Empty;
            Description = string.Empty;
            Price = 0;
            IsEditMode = false;
            SelectedSubCategory = SubCategories.FirstOrDefault();
        }

        public bool CanAdd()
        {
            return !string.IsNullOrEmpty(OrderItemName) && Price > 0 && SelectedSubCategory.Id != (int)SubCategoryType.None && !IsEditMode;
        }

        public bool CanClear()
        {
            return !string.IsNullOrEmpty(OrderItemName) || !string.IsNullOrEmpty(Description) || Price > 0 || SelectedSubCategory.Id != (int)SubCategoryType.None;
        }

        public override void LoadData()
        {
            base.LoadData();
            CategoryOrderItems = BistroDatabase.Instance.Get<CategoryOrderItem>(DBQuery.GET_CATEGORY_ORDER_ITEM).ToObservableCollection();
            IEnumerable<SubCategory> subCategory = BistroDatabase.Instance.Get<SubCategory>(DBQuery.GET_SUB_CATEGORIES);
            SubCategories.AddRange(subCategory);
        }
    }
}
