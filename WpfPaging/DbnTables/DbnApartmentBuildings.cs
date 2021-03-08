using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPaging.DbnTables
{
    class DbnApartmentBuildings
    {
          public double[,] ApartmentSpecificLoads { get; set; }
          public double[,] ElevatorsCoefOfAsk { get; set; }  
            

            public double GetApartmentSpecificLoad(double apartmentsNumber, double electrification)
            {
                double result = 0;
                double[] apartmentArr = new double[14];
                double[] firstCathegoryLoadsArr = new double[14];
                double[] thirdCathegoryLoadsArr = new double[14];

                for (int j = 0; j < apartmentArr.Length; j++)
                {
                    apartmentArr[j] = ApartmentSpecificLoads[0, j];
                    firstCathegoryLoadsArr[j] = ApartmentSpecificLoads[1, j];
                    thirdCathegoryLoadsArr[j] = ApartmentSpecificLoads[2, j];
                }

                if (electrification == 1)
                {
                    result = Interpolate(apartmentArr, firstCathegoryLoadsArr, (int)apartmentsNumber);

                }
                else
                {
                    result = Interpolate(apartmentArr, thirdCathegoryLoadsArr, (int)apartmentsNumber);

                }
                Console.WriteLine($"Результат: {result} кВт");
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
                for (byte i = 0; i < rXs.GetUpperBound(0); i++)
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
