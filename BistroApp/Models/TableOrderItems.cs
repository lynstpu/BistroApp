using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BistroApp.Models
{
    public class TableOrderItems
    {
        public int Id { get; set; }

        public int OrderItemId { get; set; }

        public int TableId { get; set; }

        public double Quantity { get; set; }

        public double TotalPrice { get; set; }

        public string TableName { get; set; }

        public string OrderItemName { get; set; }
    }
}