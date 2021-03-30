using System.ComponentModel;

namespace CCLStockChecker.Constants.Enums
{
    public enum StockEnum
    {
        [Description("InStock")]
        Instock = 1,
        [Description("OutofStock")]
        OutofStock = 2
    }
}