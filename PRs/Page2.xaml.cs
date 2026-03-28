using System;
using System.Windows;
using System.Windows.Controls;

namespace PR4
{
    /// <summary>
    /// Перечисление вариантов функции f(x) для Page2.
    /// </summary>
    public enum FunctionType
    {
        /// <summary>Гиперболический синус: f(x) = sh(x)</summary>
        Sinh,
        /// <summary>Квадрат: f(x) = x²</summary>
        Square,
        /// <summary>Экспонента: f(x) = e^x</summary>
        Exp
    }

    public partial class Page2 : Page
    {
        public Page2() => InitializeComponent();

        /// <summary>
        /// Вычисляет значение f(x) по выбранному типу функции.
        /// </summary>
        /// <param name="x">Значение аргумента.</param>
        /// <param name="type">Тип функции (Sinh, Square или Exp).</param>
        /// <returns>Значение f(x).</returns>
        public static double ComputeFx(double x, FunctionType type)
        {
            switch (type)
            {
                case FunctionType.Sinh:
                    return Math.Sinh(x);

                case FunctionType.Square:
                    return x * x;

                case FunctionType.Exp:
                    return Math.Exp(x);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        /// <summary>
        /// Вычисляет значение S в зависимости от произведения x*b:
        /// <list type="bullet">
        ///   <item><description>1 &lt; x*b &lt; 10  →  S = e^f(x)</description></item>
        ///   <item><description>12 &lt; x*b &lt; 40 →  S = √|f(x) + 4b|</description></item>
        ///   <item><description>Иначе            →  S = b * f(x)²</description></item>
        /// </list>
        /// </summary>
        /// <param name="x">Значение аргумента.</param>
        /// <param name="b">Параметр b.</param>
        /// <param name="type">Тип функции f(x).</param>
        /// <returns>Значение S, округлённое до 5 знаков.</returns>
        public static double Calculate(double x, double b, FunctionType type)
        {
            double fx = ComputeFx(x, type);
            double xb = x * b;
            double s;

            if (xb > 1 && xb < 10)
                s = Math.Exp(fx);
            else if (xb > 12 && xb < 40)
                s = Math.Sqrt(Math.Abs(fx + 4 * b));
            else
                s = b * fx * fx;

            return Math.Round(s, 5);
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtX.Text, out double x) && double.TryParse(txtB.Text, out double b))
            {
                FunctionType type = rbSh.IsChecked == true ? FunctionType.Sinh
                                  : rbSq.IsChecked == true ? FunctionType.Square
                                  : FunctionType.Exp;

                txtResult.Text = Calculate(x, b, type).ToString();
            }
            else
            {
                MessageBox.Show("Проверьте ввод чисел!");
            }
        }
    }
}
