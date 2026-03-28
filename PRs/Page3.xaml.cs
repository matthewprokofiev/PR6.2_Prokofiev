using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;

namespace PR4
{
    /// <summary>
    /// Представляет одну точку таблицы (x, y).
    /// </summary>
    public readonly struct DataPoint
    {
        /// <summary>Значение аргумента.</summary>
        public double X { get; }
        /// <summary>Значение функции.</summary>
        public double Y { get; }

        /// <summary>Инициализирует новую точку данных.</summary>
        public DataPoint(double x, double y) { X = x; Y = y; }
    }

    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            if (MyChart.ChartAreas.Count == 0)
            {
                MyChart.ChartAreas.Add(new ChartArea("MainArea"));
                var series = new Series("y = f(x)") { ChartType = SeriesChartType.Line, BorderWidth = 3 };
                MyChart.Series.Add(series);
            }
        }

        /// <summary>
        /// Вычисляет значение функции y = 9 * (x + 15 * ∛(x³ + b³)).
        /// </summary>
        /// <param name="x">Значение аргумента.</param>
        /// <param name="b">Параметр b.</param>
        /// <returns>Значение y.</returns>
        public static double ComputeY(double x, double b)
        {
            double val = Math.Pow(x, 3) + Math.Pow(b, 3);
            double cbrt = Math.Sign(val) * Math.Pow(Math.Abs(val), 1.0 / 3.0);
            return 9 * (x + 15 * cbrt);
        }

        /// <summary>
        /// Генерирует таблицу значений функции y = f(x) на отрезке [x0, xk] с шагом dx.
        /// </summary>
        /// <param name="x0">Начало отрезка.</param>
        /// <param name="xk">Конец отрезка.</param>
        /// <param name="dx">Шаг, должен быть больше нуля.</param>
        /// <param name="b">Параметр b.</param>
        /// <returns>Список точек (x, y).</returns>
        /// <exception cref="ArgumentException">
        ///   Выбрасывается, если <paramref name="dx"/> ≤ 0.
        /// </exception>
        public static List<DataPoint> BuildTable(double x0, double xk, double dx, double b)
        {
            if (dx <= 0)
                throw new ArgumentException("Шаг (dx) должен быть больше нуля!", nameof(dx));

            var points = new List<DataPoint>();
            for (double x = x0; x <= xk; x = Math.Round(x + dx, 2))
                points.Add(new DataPoint(x, Math.Round(ComputeY(x, b), 4)));

            return points;
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            bool isX0 = double.TryParse(txtX0.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double x0);
            bool isXk = double.TryParse(txtXk.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double xk);
            bool isDx = double.TryParse(txtDx.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double dx);
            bool isB = double.TryParse(txtB.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double b);

            if (!isX0 || !isXk || !isDx || !isB)
            {
                MessageBox.Show("Ошибка ввода! Убедитесь, что все поля заполнены числами.");
                return;
            }

            try
            {
                var points = BuildTable(x0, xk, dx, b);

                txtResults.Clear();
                MyChart.Series[0].Points.Clear();

                if (points.Count == 0)
                {
                    MessageBox.Show("Цикл не выполнился. Проверьте: x0 должно быть меньше xk!");
                    return;
                }

                foreach (var p in points)
                {
                    txtResults.AppendText($"x: {p.X:F2} | y: {p.Y:F4}\r\n");
                    MyChart.Series[0].Points.AddXY(p.X, p.Y);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
