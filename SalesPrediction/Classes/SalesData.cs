using System;
using Microsoft.ML.Data;

namespace SalesPrediction.Classes
{
    internal class SalesData
    {
        //[LoadColumn(0)]
        //public float Year;

        //[LoadColumn(1)]
        //public float Month;

        //[LoadColumn(3)]
        //public float Day;

        //[LoadColumn(4)]
        //public float Amount;
        [LoadColumn(0)] 
        public DateTime SalesDate { get; set; }

        [LoadColumn(1)]
        public float Amount { get; set; }

    }
}
