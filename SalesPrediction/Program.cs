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
            string _testDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\SalesPrediction\Data", "SalesData.csv");
            string _modelPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\SalesPrediction\Data", "Model.zip");

            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestPrediction(mlContext, model);

            ITransformer Train(MLContext mlContextTrain, string dataPath)
            {
                IDataView dataView = mlContext.Data.LoadFromTextFile<SalesData>(dataPath, hasHeader: true, separatorChar: ',');
                var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Amount")
                    .Append(mlContext.Transforms.Concatenate("Features", "Year", "Month", "Day"))
                    .Append(mlContext.Regression.Trainers.LbfgsPoissonRegression());
                    //.Append(mlContext.Regression.Trainers.FastTree());

                var trainingModel = pipeline.Fit(dataView);
                return trainingModel;
            }

            void Evaluate(MLContext mlContextEvaluate, ITransformer modelEvalulate)
            {
                IDataView dataView = mlContext.Data.LoadFromTextFile<SalesData>(_testDataPath, hasHeader: true, separatorChar: ',');
                var predictions = model.Transform(dataView);
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
                SalesData salesDataSample = new SalesData() { Year = 2022f, Month = 10f, Day = 29f };
                var predictionFunction = mlContextTestPrediction.Model.CreatePredictionEngine<SalesData, SalesPredictor>(model);
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
