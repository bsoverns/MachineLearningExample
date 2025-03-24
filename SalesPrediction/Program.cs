using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.TimeSeries;
using System;
using System.IO;
using System.Linq;
using SalesPrediction.Classes;
using Microsoft.ML.Transforms.TimeSeries;

namespace SalesPrediction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string _trainDataPath = Path.Combine(@"C:\Sandbox\MachineLearningExample\SalesPrediction\Data", "SalesData.csv");

            var context = new MLContext();
            var data = context.Data.LoadFromTextFile<SalesData>(_trainDataPath, hasHeader: true, separatorChar: ',');
            var pipeline = context.Forecasting.ForecastBySsa(
                    "Forecast",
                    nameof(SalesData.Amount),
                    windowSize: 5,
                    seriesLength: 10,
                    trainSize: 300,
                    horizon: 4
                );

            var model = pipeline.Fit(data);
            var forecastingEngine = model.CreateTimeSeriesEngine<SalesData, SalesForecast>(context);
            var forecasts = forecastingEngine.Predict();

            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted Sales Amount");
            Console.WriteLine($"**********************************************************************");
            
            foreach (var forecast in forecasts.Forecast)
            {
                Console.WriteLine(forecast);
            }

            Console.ReadLine();
            Console.WriteLine("");            
        }
    }
}
