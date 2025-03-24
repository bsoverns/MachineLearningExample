using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;

namespace FeedBackAnalysis
{
    internal class Program
    {
        static List<FeedBackTrainingData> trainingData = new List<FeedBackTrainingData>();
        
        static void Main(string[] args)
        {
            //1 Load
            LoadTrainingData();

            //2 Create mlContext
            var mlContext = new MLContext();

            //3 Convert Data to IDataView
            IDataView dataView = mlContext.Data.LoadFromEnumerable<FeedBackTrainingData>(trainingData);

            //4 Create Pipeline
            //  Define the workflows
            var pipeline = mlContext.Transforms.Text.FeaturizeText("FeedbackText", "Features")
                .Append(mlContext.BinaryClassification.Trainers.FastTree(numberOfLeaves: 50, numberOfTrees: 50, minimumExampleCountPerLeaf: 1));

            //5 Train algorithm
            var model = pipeline.Fit(dataView);
        }

        class FeedBackTrainingData
        {
            public string FeedBackText { get; set; }
            public bool IsGood { get; set; }

        }
        static void LoadTrainingData()
        {
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is good",
                IsGood = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is bad",
                IsGood = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is great",
                IsGood = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This sucks",
                IsGood = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is shitty",
                IsGood = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is the best",
                IsGood = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is wonderfull",
                IsGood = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is terrible",
                IsGood = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This makes me sick",
                IsGood = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is ecstatic",
                IsGood = true
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is not great",
                IsGood = false
            });
            trainingData.Add(new FeedBackTrainingData()
            {
                FeedBackText = "This is ok",
                IsGood = true
            });
        }
    }
}
