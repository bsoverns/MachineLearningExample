//https://learn.microsoft.com/en-us/dotnet/machine-learning/tutorials/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using StockPrediction.Classes;

namespace StockPrediction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string _trainDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\StockPrediction\Data", "StockDataTrain.csv");
            string _testDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\StockPrediction\Data", "StockDataTest.csv");
            string _modelPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\StockPrediction\Data", "Model.zip");

            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestingPrediction(mlContext, model);

            ITransformer Train(MLContext mlContextTrain, string dataPath)
            {
                IDataView dataView = mlContext.Data.LoadFromTextFile<StockFluctuation>(dataPath, hasHeader: true, separatorChar: ',');
                var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "ClosePrice")
                    //.Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "OpenPriceEncoded", inputColumnName: "OpenPrice"))
                    //.Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "LowPriceEncoded", inputColumnName: "LowPrice"))
                    //.Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "HighPriceEncoded", inputColumnName: "HighPrice"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "StockHistoryDateEncoded", inputColumnName: "StockHistoryDate"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "StockTickerIdEncoded", inputColumnName: "StockTickerId"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VolumeEncoded", inputColumnName: "Volume"))
                    .Append(mlContext.Transforms.Concatenate("Features", "OpenPrice", "LowPrice", "HighPrice", "StockHistoryDateEncoded", "StockTickerIdEncoded", "VolumeEncoded"))
                    .Append(mlContext.Regression.Trainers.FastTree());

                var model1 = pipeline.Fit(dataView);
                return model1;
            }

            void Evaluate(MLContext mlContextEvaluate, ITransformer modelEvalulate)
            {
                IDataView dataView = mlContext.Data.LoadFromTextFile<StockFluctuation>(_testDataPath, hasHeader: true, separatorChar: ',');
                var predictions = model.Transform(dataView);
                var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

                Console.WriteLine();
                Console.WriteLine($"*************************************************");
                Console.WriteLine($"*       Model quality metrics evaluation         ");
                Console.WriteLine($"*------------------------------------------------");
                Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
                Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
                
            }

            void TestingPrediction(MLContext mlContextTestPrediction, ITransformer modelTestPrediction)
            {
                float ActualReading = 46.06f;                

                var StockSample = new StockFluctuation()
                {

                    OpenPrice = 45.92f,
                    LowPrice = 45.62f,
                    HighPrice = 46.65f,
                    //ClosePrice = 0, // To predict. Actual/Observed = 225.09                    
                    StockHistoryDate = DateTime.Parse("10/26/2021 12:00:00 AM"),
                    StockTickerId = "PFE",
                    Volume = 21413029
                };

                var predictionFunction = mlContext.Model.CreatePredictionEngine<StockFluctuation, StockPredictor>(model);
                var prediction = predictionFunction.Predict(StockSample);               
                float difference = ActualReading - prediction.ClosePrice;
                float quotient = (difference/ prediction.ClosePrice) * 100f;

                Console.WriteLine($"**********************************************************************");
                Console.WriteLine($"Predicted Closing Price: {prediction.ClosePrice:0.####}, Actual Price: {ActualReading.ToString()} ");
                Console.WriteLine($"Off by: {difference.ToString()}, or {quotient.ToString()}%");
                Console.WriteLine($"**********************************************************************");
                Console.ReadLine();
                Console.WriteLine("");
            }
        }
    }
}
