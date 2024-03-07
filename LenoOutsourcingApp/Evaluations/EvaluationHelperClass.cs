using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EigenbelegToolAlpha
{
    public class EvaluationHelperClass
    {
        public static double ConvertPercentageStringToDouble(string input)
        {
            decimal percentage = decimal.Parse(input.TrimEnd('%')) / 100m;
            return (double)percentage;
        }

    }
}
