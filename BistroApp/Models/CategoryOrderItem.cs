using BistroApp.Enums;

namespace BistroApp.Models
{
    public class CategoryOrderItem
    {
        public int SCOId { get; set; }

        public int OrderItemId { get; set; }

        public int SubCategoryId { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public string OrderItemName { get; set; }

        public string Description { get; set; }

        public CategoryType Category { get; set; }

        public SubCategoryType SubCategory { get; set; }

        public double Price { get; set; }
    }
}
