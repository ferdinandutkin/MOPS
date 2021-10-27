using System.Diagnostics;
using Accord.Statistics;
using Accord.Math;
using Accord.Statistics.Models.Regression.Linear;
 

namespace MOPS;

using System.Linq;

class Program
{



    static void Task1()
    {




        Console.WriteLine(nameof(Task1));
        (double x1, double x2, double x3)[] table = 
        {
            (6.0, 2.0, 25.0),
            (4.9, 0.8, 30),
            (7, 2.7, 20),
            (6.7, 3, 21),
            (5.8, 1, 28),
            (6.1, 2.1, 26),
            (5, 0.9, 30),
            (6.9, 2.6, 22),
            (6.8, 3, 20),
            (5.9, 1.1, 29)
        };


        var matrix = new[]
        {
            table.Select(el => el.x1).ToArray(),
            table.Select(el => el.x2).ToArray(),
            table.Select(el => el.x3).ToArray(),
        }.Transpose();

       


        var correlationMatrix = matrix.Correlation();

        PrintSeparator();

        Console.WriteLine("Correlation: ");

        Console.WriteLine($"x1 & x2 : {correlationMatrix[0][1]}");

        Console.WriteLine($"x2 & x3 : {correlationMatrix[1][2]}");

        Console.WriteLine($"x1 & x3 : {correlationMatrix[0][2]}");

        PrintSeparator();

        Console.WriteLine();

    }


    static void Task2()
    {

        Console.WriteLine(nameof(Task2));
        (double inflation, double refinancingRate, double exchangeRate)[] table =
        {
             (84, 85, 441),
             (45, 55, 980),
             (56, 65, 1400),
             (34, 40, 1960),
             (23, 28, 2030)
        };


        var matrix  = new[]
        {
            table.Select(el => el.inflation).ToArray(),
            table.Select(el => el.refinancingRate).ToArray(),
            table.Select(el => el.exchangeRate).ToArray(),
        }.Transpose();

        

        var correlationMatrix = matrix.Correlation();


        PrintSeparator();

        Console.WriteLine("Correlation: ");

        Console.WriteLine($"inflation & refinancingRate : {correlationMatrix[0][1]}");

        Console.WriteLine($"refinancingRate & exchangeRate : {correlationMatrix[1][2]}");

        Console.WriteLine($"inflation & exchangeRate : {correlationMatrix[0][2]}");

        PrintSeparator();


        Console.WriteLine();
    }


    static void PrintSeparator()
    {
        Console.WriteLine(new string('-', 60));
    }

    static void Task3()
    {



        (double y, double x1, double x2)[] table =
        {
            (1.3, 1.1, 20),
            (2.3, 19, 14),
            (1.8, 13, 12),
            (1.4, 14, 8),
            (1.1, 11, 10),
            (1.2, 17, 6),
            (2.7, 23, 16),
            (1.9, 11, 15),
            (1.5, 13, 8),
            (2.1, 20, 17),
            (1.7, 15, 12)
        };




        Console.WriteLine(nameof(Task3));

        var y = table.Select(e => e.y).ToArray();
        var x1 = table.Select(e => e.x1).ToArray();
        var x2 = table.Select(e => e.x2).ToArray();



        var inputs = new[] { x1, x2 }.Transpose();
        var output = y;


        var regression = new OrdinaryLeastSquares().Learn(inputs, output);


        Console.WriteLine(regression.ToString().Replace("x1", "x2").Replace("x0", "x1"));


        var (k1, k2, k3) = (regression.Weights[0], regression.Weights[1], regression.Intercept);
        var rSqr = regression.CoefficientOfDetermination(inputs, output);

        var N = table.Length;
        var p = 2; //число независимых переменных
        var rSqrAdjusted = 1 - (1 - rSqr) * (N - 1) / (N - p - 1);


   


        Console.WriteLine($"Coefficients: {(k1, k2, k3)}");
        Console.WriteLine($"R^2: {rSqr}");
        Console.WriteLine($"Adjusted R^2: {rSqrAdjusted}");
      



        double income1 = 15;
        double assets1 = 18;
        double savings1 = regression.Transform(new[] { income1, assets1 });
        Console.WriteLine($"Income1: ${income1}, Assets1: ${assets1} => Savings1 : ${savings1}");
 

        double dIncome1 = 5;
        double income2 = income1 + dIncome1;
        double assets2 = assets1;
        double savings2 = savings1 + (k1 * dIncome1);
        Debug.Assert(Math.Abs(savings2 - regression.Transform(new[] { income2, assets2 })) < 0.01);
        Console.WriteLine($"Income2: ${income2}, Assets2: ${assets2} => Savings2 : ${savings2}, Savings2/Savings1 = {savings2 / savings1}");
 


        double dIncome2 = 3;
        double dAssets2 = 5;
        double income3 = income2 + dIncome2;
        double assets3 = assets2 + dAssets2;
        double savings3 = savings2 + (k1 * dIncome2) + (k2 * dAssets2);
        Debug.Assert(Math.Abs(savings3 - regression.Transform(new[] { income3, assets3 })) < 0.01);
        Console.WriteLine($"Income3: ${income3}, Assets3: ${assets3} => Savings3 : ${savings3}, Savings3/Savings2 = {savings3 / savings2}");
 


        double dIncome3 = income2 * 0.1;
        double income4 = income3 + dIncome3;
        double assets4 = assets3;
        double savings4 = savings3 + (k1 * dIncome3);
        Debug.Assert(Math.Abs(savings4 - regression.Transform(new[] { income4, assets4 })) < 0.01);
        Console.WriteLine($"Income4: ${income4}, Assets4: ${assets4} => Savings4 : ${savings4}, Savings4/Savings3 = {savings4 / savings3}");


        var matrix = new[]
        {
            y, x1, x2
        }.Transpose();


     
        var correlationMatrix = matrix.Correlation();

        PrintSeparator();

        Console.WriteLine("Correlation: ");
        Console.WriteLine($"income & savings : {correlationMatrix[1][0]}");

        Console.WriteLine($"assets & savings : {correlationMatrix[2][0]}");

        Console.WriteLine($"income & assets : {correlationMatrix[1][2]}");
        PrintSeparator();

        var higherKName = (k1 > k2) ? nameof(x1) : nameof(x2);

        Console.WriteLine($"{higherKName} has higher coefficient so it affects more");
   


        Console.WriteLine($"0.95 > Adjusted R^2 ({rSqrAdjusted}) > 0.8");
 





    }
    static void Main(string[] args)
    {
        Task1();

        Task2();

        Task3();
    }
}

