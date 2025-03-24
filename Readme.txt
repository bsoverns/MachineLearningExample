C# Microsoft.ML
Other examples - https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet#:~:text=With%20ML.NET%2C%20you%20can%20create%20custom%20ML%20models%20using,%2C%20games%2C%20and%20IoT%20apps.

General Setup
//Step 1. Create an ML Context
var ctx = new MLContext();

//Step 2. Read in the input data from a text file for model training
IDataView trainingData = ctx.Data
    .LoadFromTextFile<ModelInput>(dataPath, hasHeader: true);

//Step 3. Build your data processing and training pipeline
var pipeline = ctx.Transforms.Text
    .FeaturizeText("Features", nameof(SentimentIssue.Text))
    .Append(ctx.BinaryClassification.Trainers
        .LbfgsLogisticRegression("Label", "Features"));

//Step 4. Train your model
ITransformer trainedModel = pipeline.Fit(trainingData);

//Step 5. Make predictions using your trained model
var predictionEngine = ctx.Model
    .CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);

var sampleStatement = new ModelInput() { Text = "This is a horrible movie" };

var prediction = predictionEngine.Predict(sampleStatement);

IrisPrediction
https://www.datarmatics.com/data-science/ml-net-tutorial-perform-cluster-analysis-using-iris-dataset/
https://www.youtube.com/watch?v=oPckdACVhAI
Retrain - https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/retrain-model-ml-net

SalaryPrediction
https://www.youtube.com/watch?v=8gVhJKszzzI&list=PLl_upHIj19Zy3o09oICOutbNfXj332czx
Retrain - https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/retrain-model-ml-net

TaxiPrediction
https://learn.microsoft.com/en-us/dotnet/machine-learning/tutorials/
https://learn.microsoft.com/en-us/dotnet/machine-learning/tutorials/predict-prices

StockPrediction
https://learn.microsoft.com/en-us/dotnet/machine-learning/tutorials/
Re-train - https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/retrain-model-ml-net

Misc Examples watched:
Sales Figure predictions: https://www.youtube.com/watch?v=aPN4J2eK9O0