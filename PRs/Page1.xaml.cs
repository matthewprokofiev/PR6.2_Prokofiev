using System;
using System.Windows;
using System.Windows.Controls;

namespace PR4
{
    public partial class Page1 : Page
    {
        public Page1() => InitializeComponent();

        /// <summary>
        /// Вычисляет значение математической функции:
        /// F(x, y, z) = 5*arctg(x) - 0.25*arccos(x) * (x + 3*|x-y| + x²) / (|x-y|*z + x²)
        /// </summary>
        /// <param name="x">Аргумент, должен быть в диапазоне [-1, 1].</param>
        /// <param name="y">Второй аргумент функции.</param>
        /// <param name="z">Третий аргумент функции.</param>
        /// <returns>Значение функции, округлённое до 6 знаков после запятой.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Выбрасывается, если <paramref name="x"/> не принадлежит [-1, 1].
        /// </exception>
        /// <exception cref="DivideByZeroException">
        ///   Выбрасывается, если знаменатель равен нулю.
        /// </exception>
        public static double Calculate(double x, double y, double z)
        {
            if (x < -1 || x > 1)
                throw new ArgumentOutOfRangeException(nameof(x), "x для arccos должен быть в диапазоне [-1, 1]");

            double znam = Math.Abs(x - y) * z + Math.Pow(x, 2);
            if (znam == 0)
                throw new DivideByZeroException("Знаменатель равен нулю!");

            double chisl = x + 3 * Math.Abs(x - y) + Math.Pow(x, 2);
            double res = 5 * Math.Atan(x) - 0.25 * Math.Acos(x) * (chisl / znam);

            return Math.Round(res, 6);
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtX.Text) || string.IsNullOrEmpty(txtY.Text) || string.IsNullOrEmpty(txtZ.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (double.TryParse(txtX.Text, out double x) &&
                double.TryParse(txtY.Text, out double y) &&
                double.TryParse(txtZ.Text, out double z))
            {
                try
                {
                    txtResult.Text = Calculate(x, y, z).ToString();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка ОДЗ");
                }
                catch (DivideByZeroException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Введите числовые значения!");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtX.Clear(); txtY.Clear(); txtZ.Clear(); txtResult.Clear();
        }
    }
}
