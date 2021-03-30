using System;
using System.ComponentModel.DataAnnotations;
using CCLStockChecker.Constants.Enums;
#nullable enable

namespace CCLStockChecker.Models
{
    public class ProductModel
    {
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Link { get; set; }
        public StockEnum? InStock { get; set; }


    }
}