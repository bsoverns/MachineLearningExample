using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPrediction.Classes
{
    public class StockFluctuation
    {
        [LoadColumn(0)]
        public float OpenPrice;

        [LoadColumn(1)]
        public float LowPrice;

        [LoadColumn(2)]
        public float HighPrice;

        [LoadColumn(3)]
        public float ClosePrice;

        [LoadColumn(4)]
        public DateTime StockHistoryDate;

        [LoadColumn(5)]
        public string StockTickerId;

        [LoadColumn(6)]
        public int Volume;
    }

    public class StockPredictor
    {
        [ColumnName("Score")]
        public float ClosePricePrediction;
    }
}
