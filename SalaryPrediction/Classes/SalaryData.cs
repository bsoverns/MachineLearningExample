using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryPrediction.Classes
{
    public class SalaryData
    {

        [LoadColumn(0)]
        public float YearsExperience;

        [LoadColumn(1)]
        public float Salary;
    }

    public class SalaryPredictor
    {
        [ColumnName("Score")]
        public float PredictedSalary;
    }
}
