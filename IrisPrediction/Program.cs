//https://www.datarmatics.com/data-science/ml-net-tutorial-perform-cluster-analysis-using-iris-dataset/
//https://www.youtube.com/watch?v=oPckdACVhAI
//Retrain - https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/retrain-model-ml-net

using System;
using System.IO;
using Microsoft.ML;

namespace IrisPrediction
{
    internal class Program
    {
        //static readonly string _datapath = Path.Combine(Environment.CurrentDirectory, "Data", "stock.data");
        //static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "StockClusteringModel.zip");
        static readonly string _datapath = Path.Combine(@"D:\Sandbox\MachineLearningExample\IrisPrediction\Data", "iris.data");
        static readonly string _modelPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\IrisPrediction\Data", "IrisClusteringModel.zip");

        static void Main(string[] args)
        {
            var mlContext = new MLContext(seed: 0);

            IDataView dataView = mlContext.Data.LoadFromTextFile<IrisData>(_datapath, hasHeader: false, separatorChar: ',');

            string featuresColumnName = "Features";

            var pipeline = mlContext.Transforms
                //.Concatenate(StockColumnName, "OpenPrice", "LowPrice", "HighPrice", "ClosePrice").Append(mlContext.Clustering.Trainers.KMeans(StockColumnName, numberOfClusters: 4));
                .Concatenate(featuresColumnName,
                "SepalLength",
                "SepalWidth",
                "PetalLength",
                "PetalWidth")
                .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: 3));

            var model = pipeline.Fit(dataView);

            using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(model, dataView.Schema, fileStream);
            }

            var predictor = mlContext.Model.CreatePredictionEngine<IrisData, ClusterPrediction>(model);

            var prediction = predictor.Predict(TestIrisData.Setosa);
            Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
            Console.WriteLine($"Distances: {string.Join(" ", prediction.Distances)}");
            Console.ReadLine();
            Console.WriteLine("Hello");
        }
    }
}
