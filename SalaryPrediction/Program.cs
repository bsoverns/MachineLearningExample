//https://www.youtube.com/watch?v=8gVhJKszzzI&list=PLl_upHIj19Zy3o09oICOutbNfXj332czx
//Retrain - https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/retrain-model-ml-net

using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using SalaryPrediction.Classes;

namespace SalaryPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            string _trainDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\SalaryPrediction\Data", "SalaryData.csv");
            string _testDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\SalaryPrediction\Data", "SalaryData-test.csv");
            string _modelPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\SalaryPrediction\Data", "Model.zip");

            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestPrediction(mlContext, model);

            ITransformer Train(MLContext mlContextTrain, string dataPath)
            {
                IDataView dataView = mlContext.Data.LoadFromTextFile<SalaryData>(dataPath, hasHeader: true, separatorChar: ',');
                var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Salary")
                    .Append(mlContext.Transforms.Concatenate("Features", "YearsExperience"))
                    .Append(mlContext.Regression.Trainers.LbfgsPoissonRegression());

                var model1 = pipeline.Fit(dataView);
                return model1;
            }

            void Evaluate(MLContext mlContextEvaluate, ITransformer modelEvalulate)
            {
                IDataView dataView = mlContext.Data.LoadFromTextFile<SalaryData>(_testDataPath, hasHeader: true, separatorChar: ',');
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
                SalaryData salaryData = new SalaryData { YearsExperience = 7 };
                var predictionFunction = mlContextTestPrediction.Model.CreatePredictionEngine<SalaryData, SalaryPredictor>(model);
                var prediction = predictionFunction.Predict(salaryData);

                Console.WriteLine($"**********************************************************************");
                Console.WriteLine($"Predicted Closing Price: {prediction.PredictedSalary:0.####}");                
                Console.WriteLine($"**********************************************************************");
                Console.ReadLine();
                Console.WriteLine("");
            }
        }
    }
}
