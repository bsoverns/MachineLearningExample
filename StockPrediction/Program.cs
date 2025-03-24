using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using StockPrediction.Classes;

namespace StockPrediction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string _trainDataPath = Path.Combine(@"C:\Sandbox\MachineLearningExample\StockPrediction\Data", "StockDataTrain.csv");
            string _testDataPath = Path.Combine(@"C:\Sandbox\MachineLearningExample\StockPrediction\Data", "StockDataTest.csv");
            string _modelPath = Path.Combine(@"C:\Sandbox\MachineLearningExample\StockPrediction\Data", "Model.zip");

            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestingPrediction(mlContext, model);

            ITransformer Train(MLContext mlContextTrain, string dataPath)
            {
                IDataView dataView = mlContextTrain.Data.LoadFromTextFile<StockFluctuation>(dataPath, hasHeader: true, separatorChar: ',');
                var pipeline = mlContextTrain.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "ClosePrice")
                    //.Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "OpenPriceEncoded", inputColumnName: "OpenPrice"))
                    //.Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "LowPriceEncoded", inputColumnName: "LowPrice"))
                    //.Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "HighPriceEncoded", inputColumnName: "HighPrice"))
                    .Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "StockHistoryDateEncoded", inputColumnName: "StockHistoryDate"))
                    .Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "StockTickerIdEncoded", inputColumnName: "StockTickerId"))
                    .Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "VolumeEncoded", inputColumnName: "Volume"))
                    .Append(mlContextTrain.Transforms.Concatenate("Features", "OpenPrice", "LowPrice", "HighPrice", "StockHistoryDateEncoded", "StockTickerIdEncoded", "VolumeEncoded"))
                    .Append(mlContextTrain.Regression.Trainers.FastTree());

                var modelTrain = pipeline.Fit(dataView);
                return modelTrain;
            }

            void Evaluate(MLContext mlContextEvaluate, ITransformer modelEvalulate)
            {
                IDataView dataView = mlContextEvaluate.Data.LoadFromTextFile<StockFluctuation>(_testDataPath, hasHeader: true, separatorChar: ',');
                var predictions = modelEvalulate.Transform(dataView);
                var metrics = mlContextEvaluate.Regression.Evaluate(predictions, "Label", "Score");

                Console.WriteLine();
                Console.WriteLine($"*************************************************");
                Console.WriteLine($"*       Model quality metrics evaluation         ");
                Console.WriteLine($"*------------------------------------------------");
                Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
                Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
                
            }

            void TestingPrediction(MLContext mlContextTestPrediction, ITransformer modelTestPrediction)
            {
                float ActualReading = 71.96f;                

                var StockSample = new StockFluctuation()
                {

                    OpenPrice = 72.08f,
                    LowPrice = 71.45f,
                    HighPrice = 72.34f,
                    ClosePrice = 0, // To predict. Actual/Observed = 225.09                    
                    StockHistoryDate = DateTime.Parse("11/04/2022 12:00:00 AM"),
                    StockTickerId = "ATVI",
                    Volume = 4333117
                };

                var predictionFunction = mlContextTestPrediction.Model.CreatePredictionEngine<StockFluctuation, StockPredictor>(modelTestPrediction);
                var prediction = predictionFunction.Predict(StockSample);               
                float difference = ActualReading - prediction.ClosePricePrediction;
                float quotient = (difference/ prediction.ClosePricePrediction) * 100f;

                Console.WriteLine($"**********************************************************************");
                Console.WriteLine($"Predicted Closing Price: {prediction.ClosePricePrediction:0.####}, Actual Price: {ActualReading.ToString()} ");
                Console.WriteLine($"Off by: {difference.ToString()}, or {quotient.ToString()}%");
                Console.WriteLine($"**********************************************************************");
                Console.ReadLine();
                Console.WriteLine("");
            }
        }
    }
}
