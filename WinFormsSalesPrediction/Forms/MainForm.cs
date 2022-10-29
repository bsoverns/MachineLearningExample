//Doesn't work yet
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsSalesPrediction.Classes;

namespace WinFormsSalesPrediction
{
    public partial class MainForm : Form
    {
        string _trainDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\WinFormsSalesPrediction\Data", "SalesData.csv");
        string _testDataPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\WinFormsSalesPrediction\Data", "SalesData.csv");
        string _modelPath = Path.Combine(@"D:\Sandbox\MachineLearningExample\WinFormsSalesPrediction\Data", "Model.zip");        

        public MainForm()
        {
            InitializeComponent();
            //InitalizeModel();
        }

        void InitalizeModel()
        {
            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);

            ITransformer Train(MLContext mlContextTrain, string dataPath)
            {
                IDataView dataView = mlContext.Data.LoadFromTextFile<SalesData>(dataPath, hasHeader: true, separatorChar: ',');
                var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Amount")
                    .Append(mlContext.Transforms.Concatenate("Features", "Year", "Month", "Day"))
                    .Append(mlContext.Regression.Trainers.LbfgsPoissonRegression());

                var trainModel = pipeline.Fit(dataView);

                using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    mlContext.Model.Save(trainModel, dataView.Schema, fileStream);
                }

                return trainModel;
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
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            float year;
            float month;
            float day;

            if (IsValid(txtYear.Text) && IsValid(txtMonth.Text) && IsValid(txtDay.Text))
            {
                year = Convert.ToSingle(txtYear.Text);
                month = Convert.ToSingle(txtMonth.Text);
                day = Convert.ToSingle(txtDay.Text);

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

                    var model1 = pipeline.Fit(dataView);

                    using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        mlContext.Model.Save(model1, dataView.Schema, fileStream);
                    }

                    return model1;
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
                    var SalesDataSample = new SalesData() 
                    {
                        Year = year,
                        Month = month,
                        Day = day,
                        Amount = 0
                    };

                    var predictionFunction = mlContext.Model.CreatePredictionEngine<SalesData, SalesPrediction>(model);
                    var prediction = predictionFunction.Predict(SalesDataSample);

                    lblPrediction.Text = "Prediction: " + prediction.SalesAmount.ToString();
                    lblPrediction.Visible = true;
                    //Console.WriteLine($"**********************************************************************");
                    //Console.WriteLine($"Predicted fare: {prediction.SalesAmount:0.####}, actual amount: 1234");
                    //Console.WriteLine($"**********************************************************************");
                    //Console.ReadLine();
                    //Console.WriteLine("");
                }
            }                           

            else
                MessageBox.Show("Missing Data!");
        }

        public bool IsValid(string input)
        {
            if (input != "")
                return true;

            else
                return false;                  
        }
    }
}
