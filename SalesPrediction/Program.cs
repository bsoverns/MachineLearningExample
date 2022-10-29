using Microsoft.ML;
using System;
using System.IO;
using System.Linq;
using SalesPrediction.Classes;

namespace SalesPrediction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string _trainDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\SalesPrediction\Data", "SalesData.csv");
            string _testDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\SalesPrediction\Data", "SalesData-test.csv");
            string _modelPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\SalesPrediction\Data", "Model.zip");

            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestPrediction(mlContext, model);

            ITransformer Train(MLContext mlContextTrain, string dataPath)
            {
                IDataView dataView = mlContextTrain.Data.LoadFromTextFile<SalesData>(dataPath, hasHeader: true, separatorChar: ',');
                var pipeline = mlContextTrain.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Amount")
                    .Append(mlContextTrain.Transforms.Concatenate("Features", "Day"))
                    //.Append(mlContextTrain.Regression.Trainers.LbfgsPoissonRegression());
                    .Append(mlContextTrain.Regression.Trainers.FastTree());

                var trainingModel = pipeline.Fit(dataView);

                using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    mlContextTrain.Model.Save(trainingModel, dataView.Schema, fileStream);
                }

                return trainingModel;
            }

            void Evaluate(MLContext mlContextEvaluate, ITransformer modelEvalulate)
            {
                IDataView dataView = mlContextEvaluate.Data.LoadFromTextFile<SalesData>(_testDataPath, hasHeader: true, separatorChar: ',');
                var predictions = modelEvalulate.Transform(dataView);
                var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

                Console.WriteLine();
                Console.WriteLine($"*************************************************");
                Console.WriteLine($"*       Model quality metrics evaluation         ");
                Console.WriteLine($"*------------------------------------------------");
                Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
                Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
            }

            void TestPrediction(MLContext mlContextTestPrediction, ITransformer modelTestPrediction)
            {
                SalesData salesDataSample = new SalesData() { Year = 2022, Month = 10, Day = 27 };
                var predictionFunction = mlContextTestPrediction.Model.CreatePredictionEngine<SalesData, SalesPredictor>(modelTestPrediction);
                var prediction = predictionFunction.Predict(salesDataSample);

                Console.WriteLine($"**********************************************************************");
                Console.WriteLine($"Predicted Sales Amount: {prediction.SalesAmountPrediction:0.####}");
                Console.WriteLine($"**********************************************************************");
                Console.ReadLine();
                Console.WriteLine("");
            }
        }
    }
}
