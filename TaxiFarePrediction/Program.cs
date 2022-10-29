using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using TaxiFarePrediction.Classes;

namespace TaxiFarePrediction
{
    internal class Program
    {    
        static void Main(string[] args)
        {
            string _trainDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\TaxiFarePrediction\Data", "taxi-fare-train.csv");
            string _testDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\TaxiFarePrediction\Data", "taxi-fare-test.csv");
            string _modelPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\TaxiFarePrediction\Data", "Model.zip");

            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestSinglePrediction(mlContext, model);

            ITransformer Train(MLContext mlContextTrain, string dataPath)
            {
                IDataView dataView = mlContextTrain.Data.LoadFromTextFile<TaxiTrip>(dataPath, hasHeader: true, separatorChar: ',');
                var pipeline = mlContextTrain.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount")
                    .Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName: "VendorId"))
                    .Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
                    .Append(mlContextTrain.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
                    .Append(mlContextTrain.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded", "PassengerCount", "TripDistance", "PaymentTypeEncoded"))
                    .Append(mlContextTrain.Regression.Trainers.FastTree());

                var modelTrain = pipeline.Fit(dataView);
                return modelTrain;
            }

            void Evaluate(MLContext mlContextEvaluate, ITransformer modelEvalulate)
            {
                IDataView dataView = mlContextEvaluate.Data.LoadFromTextFile<TaxiTrip>(_testDataPath, hasHeader: true, separatorChar: ',');
                var predictions = modelEvalulate.Transform(dataView);
                var metrics = mlContextEvaluate.Regression.Evaluate(predictions, "Label", "Score");

                Console.WriteLine();
                Console.WriteLine($"*************************************************");
                Console.WriteLine($"*       Model quality metrics evaluation         ");
                Console.WriteLine($"*------------------------------------------------");
                Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
                Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
            }

            void TestSinglePrediction(MLContext mlContextTestPrediction, ITransformer modelTestPrediction)
            {
                var taxiTripSample = new TaxiTrip()
                {
                    VendorId = "VTS",
                    RateCode = "1",
                    PassengerCount = 1,
                    TripTime = 1140f,
                    TripDistance = 3.75f,
                    PaymentType = "CRD",
                    FareAmount = 0 // To predict. Actual/Observed = 15.5
                };

                var predictionFunction = mlContextTestPrediction.Model.CreatePredictionEngine<TaxiTrip, TaxiTripFarePrediction>(modelTestPrediction);
                var prediction = predictionFunction.Predict(taxiTripSample);

                Console.WriteLine($"**********************************************************************");
                Console.WriteLine($"Predicted fare: {prediction.FareAmount:0.####}, actual fare: 15.5");
                Console.WriteLine($"**********************************************************************");
                Console.ReadLine();
                Console.WriteLine("");
            }
        }
    }
}
