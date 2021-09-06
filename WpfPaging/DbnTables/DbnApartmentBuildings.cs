using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.DistrictObjects;

namespace WpfPaging.DbnTables
{
    public class DbnApartmentBuildings
    {

        public double[,] ApartmentSpecificLoads { get; set; }
        public double[,] ElevatorsCoefOfAsk { get; set; }
        public double[,] PompsCoefOfAsk { get; set; }
       

        public TgFi tgFi = new TgFi();

        // Задаю константы коэффициентов нагрузки
        public class TgFi
        {
         public double Pomps = 1.17;
         public double Elevators = 0.75;
         public double FirstCathegoryApartments = 0.2;
         public double ThirdCathegoryApartments = 0.29;
        }


        public double GetApartmentSpecificLoad(double apartmentsNumber, double electrification, double[,] dbnData)
        {
            double result = 0;
            double[] apartmentArr = new double[dbnData.GetUpperBound(1) + 1];
            double[] firstCathegoryLoadsArr = new double[dbnData.GetUpperBound(1) + 1];
            double[] thirdCathegoryLoadsArr = new double[dbnData.GetUpperBound(1) + 1];

            for (int j = 0; j < apartmentArr.Length; j++)
            {
                apartmentArr[j] = dbnData[0, j];
                firstCathegoryLoadsArr[j] = dbnData[1, j];
                thirdCathegoryLoadsArr[j] = dbnData[2, j];
            }

            if (electrification == 1)
            {
                try
                {
                    result = Interpolate(apartmentArr, firstCathegoryLoadsArr, (int)apartmentsNumber);
                }
                catch
                {
                    if (apartmentsNumber >= 1000) result = 0.6;
                }
            }
            else
            {
                try
                {
                    result = Interpolate(apartmentArr, thirdCathegoryLoadsArr, (int)apartmentsNumber);
                }
                catch
                {
                    if (apartmentsNumber >= 1000) result = 1.1;
                }
            }
            Console.WriteLine($"Результат: {result} кВт");
            result = Math.Round(result, 2);
            return result;
        }


        public double GetElevatorCoefficientofAsk(double elevators, double levels, double[,] dbnData)
        {
            double result = 0;
            double[] elevatorsArr = new double[dbnData.GetUpperBound(1) + 1];
            double[] lowerThan12 = new double[dbnData.GetUpperBound(1) + 1];
            double[] equalTo12OrHigher = new double[dbnData.GetUpperBound(1) + 1];

            for (int j = 0; j < elevatorsArr.Length; j++)
            {
                elevatorsArr[j] = dbnData[0, j];
                lowerThan12[j] = dbnData[1, j];
                equalTo12OrHigher[j] = dbnData[2, j];
            }

            if (levels < 12)
            {
                if (elevators > 24)
                { result = 0.35; return result; }
                // Добавил выбор максимального числа
                result = Interpolate(elevatorsArr, lowerThan12, (int)elevators);
            }

            else
            {
                if (elevators > 24)
                { result = 0.4; return result; }
                // Добавил выбор макс числа
                result = Interpolate(elevatorsArr, equalTo12OrHigher, (int)elevators);
            }
            Console.WriteLine($"Результат: {result} кВт");
            result = Math.Round(result, 2);
            return result;

        }
  
        /// <summary>
        /// Функция выдаёт коэфициент спроса на насосы
        /// </summary>
        /// <param name="pompsNumber"> Число насосов</param>
        /// <param name="pompPercentage"> Какую часть в общей удельной нагрузке занимают насосы</param>
        /// <param name="dbnData"> Таблица с коэфициентами спроса</param>
        /// <returns></returns>
        public double GetPompsCoefficientofAsk(double pompsNumber, double pompPercentage,  double[,] dbnData, int numberOfElevators)
        {
            
            
            double result = 0;
            double[] pompsNumberArr = new double[dbnData.GetUpperBound(1) + 1];
            double[] loadAbove84 = new double[dbnData.GetUpperBound(1) + 1];
            double[] loadAbove74 = new double[dbnData.GetUpperBound(1) + 1];
            double[] loadAbove49 = new double[dbnData.GetUpperBound(1) + 1];
            double[] loadAbove24 = new double[dbnData.GetUpperBound(1) + 1];
            double[] loadBelow24 = new double[dbnData.GetUpperBound(1) + 1];

            for (int j = 0; j < pompsNumberArr.Length; j++)
            {
                pompsNumberArr[j] = dbnData[0, j];
                loadAbove84[j] = dbnData[1, j];
                loadAbove74[j] = dbnData[2, j];
                loadAbove49[j] = dbnData[3, j];
                loadAbove24[j] = dbnData[4, j];
                loadBelow24[j] = dbnData[5, j];
                // Описываю случай когда искомое значение находится среди табличных данных
                if (pompsNumberArr[j]==(pompsNumber+numberOfElevators) && pompsNumber != 0)
                {
                    if (pompPercentage >= 84)
                    { result = loadAbove84[j]; }
                    else if (pompPercentage >= 74)
                    { result = loadAbove74[j]; }
                    else if (pompPercentage >= 49)
                    { result = loadAbove49[j]; }
                    else if (pompPercentage >= 24)
                    { result = loadAbove24[j]; }
                    else if (pompPercentage < 24)
                    { result = loadBelow24[j]; }
                    return result;
                }
                else if (pompsNumber ==0)
                { result = 0; return result; }
            }

    

            if (pompPercentage>84)
            {
                result = Interpolate(pompsNumberArr, loadAbove84, (int)pompsNumber+numberOfElevators);
              
            }

            else if (pompPercentage>74)
            {
                result = Interpolate(pompsNumberArr, loadAbove74, (int)pompsNumber + numberOfElevators);
            }

            else if (pompPercentage > 49)
            {
                result = Interpolate(pompsNumberArr, loadAbove49, (int)pompsNumber + numberOfElevators);
            }

            else if (pompPercentage >= 25)
            {
                result = Interpolate(pompsNumberArr, loadAbove24, (int)pompsNumber + numberOfElevators);
            }

            else if (pompPercentage < 25)
            {
                result = Interpolate(pompsNumberArr, loadBelow24, (int)pompsNumber + numberOfElevators);
            }
            Console.WriteLine($"Результат: {result} кВт");
            result = Math.Round(result, 2);
            return result;

        }




        public DbnApartmentBuildings()
        {
            ApartmentSpecificLoads = new double[,]
            { 
                // Число квартир
                { 1,  3,    6,    9,    12,   15,   18,   24,   40,   60,   100,  200,   400,   600,   1000 },
                // Нагрузка квартир кат 1
                { 5,  3.85, 3.23, 2.72, 2.36, 2.10, 1.91, 1.65, 1.31, 1.14, 1,   0.87,  0.74,  0.66,  0.6 },
                // Нагрузка квартир кат 3
                { 10, 8.19, 5.56, 4.44, 3.76, 3.33, 3.05, 2.72, 2.35, 2.10, 1.73, 1.38,  1.31,  1.19,  1.10}
            };

            ElevatorsCoefOfAsk = new double[,]
            {
                // Количество лифтовых установок
                {2 ,  3,   4,   5,   6,    10,  20,  25,   100 },
                // До 12 этажей
                {0.8, 0.8, 0.7, 0.7, 0.65, 0.5, 0.4, 0.35, 0.35},
                // 12 и более
                {0.9, 0.9, 0.8, 0.8, 0.75, 0.6, 0.5, 0.4, 0.4 }
            };

            PompsCoefOfAsk = new double[,]
            {
                // Кількість електроприймачів
                {1, 2, 3,   5,    8,      10,    15,    20,    30,   50,     100 },
                // 100-85                              
                {1, 1, 0.9, 0.8,  0.75,   0.7,   0.65,  0.65,  0.6,   0.55,  0.55},
                //84-75                                
                {1, 1, 1,   0.75, 0.7,   0.65,   0.6,  0.6,    0.6,  0.55,   0.55 },
                // 74-50                        
                {1, 1, 1,   0.7,  0.65,   0.65,  0.6,   0.6,   0.55,  0.50,  0.5 },
                // 49-25                 
                {1, 1, 1,   0.65, 0.6,   0.6,    0.55,  0.5,   0.5,  0.5,    0.45 },
                // 24 і менше            
                {1, 1, 1,   0.6,  0.6,   0.55,   0.5,  0.5,    0.5,  0.45,    0.45 }
            };

          

            TgFi tgFi = new TgFi();
        }

        /// <summary>
        /// Производит сплайн интерполяцию
        /// </summary>
        /// <param name="rXs">Массив параметров оси Х, горизонтальный ряд таблицы</param>
        /// <param name="rYs">Массив параметров оси У, вертикальный ряд таблицы</param>
        /// <param name="sX">Координата искомой токчки Х</param>
        /// <returns></returns>
        public double Interpolate(double[] rXs, double[] rYs, int sX)
        {
            double sY = 0;
            for (byte i = 0; i < rXs.GetUpperBound(0);)
            {
                int size = (int)rXs[0];
                for (int j = 1; j < rXs.Length; j++)
                {
                    if (rXs[j] > size)
                        size = (int)rXs[j];
                }

                double[] rInterpolated = new double[size];

                for (int splineIndex = 0; splineIndex < rXs.Length - 1; splineIndex++)
                {
                    double x1 = rXs[splineIndex];
                    double x2 = rXs[splineIndex + 1];
                    double y1 = rYs[splineIndex];
                    double y2 = rYs[splineIndex + 1];

                    double distance = x2 - x1;
                    for (int l = 0; l < distance; l++)
                    {
                        double muLinear = (double)l / distance;
                        double muCos = (1 - Math.Cos(muLinear * Math.PI)) / 2;

                        double mu = muCos;
                        rInterpolated[(int)x1 + l] = (y1 * (1 - mu) + y2 * mu);
                    }
                }
                sY = rInterpolated[sX];
                return sY;
            }
            return sY;


        }
    }

}
