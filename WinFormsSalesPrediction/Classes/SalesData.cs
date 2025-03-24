using Microsoft.ML.Data;

namespace WinFormsSalesPrediction.Classes
{
    public class SalesData
    {
        [LoadColumn(0)]
        public float Year;
        
        [LoadColumn(1)]
        public float Month;

        [LoadColumn(3)]
        public float Day;

        [LoadColumn(4)]
        public float Amount;
    }

    public class SalesPrediction
    {
        [ColumnName("Score")]
        public float SalesAmount;
    }
}
