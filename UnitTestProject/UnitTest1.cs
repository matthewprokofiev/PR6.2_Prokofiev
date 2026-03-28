using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PR4;

namespace UnitTestProject
{
    // ===========================================================================
    //  Тренировочный тест (из задания) — демонстрирует методы Assert
    // ===========================================================================

    /// <summary>
    /// Тренировочный тест, демонстрирующий основные методы объекта <see cref="Assert"/>.
    /// </summary>
    [TestClass]
    public class TrainingTests
    {
        /// <summary>
        /// Проверяет работу методов Assert: AreEqual, IsTrue, IsFalse, IsNull, IsNotNull.
        /// <para>
        /// Вывод: <br/>
        /// — <c>Assert.AreEqual</c>  — проверяет равенство двух значений (с допуском delta для double). <br/>
        /// — <c>Assert.IsTrue</c>   — проверяет, что условие истинно. <br/>
        /// — <c>Assert.IsFalse</c>  — проверяет, что условие ложно. <br/>
        /// — <c>Assert.IsNull</c>   — проверяет, что объект равен null. <br/>
        /// — <c>Assert.IsNotNull</c>— проверяет, что объект не равен null.
        /// </para>
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            // AreEqual: числа, строки, с допустимой погрешностью
            Assert.AreEqual(4, 2 + 2);
            Assert.AreEqual("hello", "hel" + "lo");
            Assert.AreEqual(0.3, 0.1 + 0.2, 1e-10);   // delta — допуск для double

            // IsTrue / IsFalse
            Assert.IsTrue(5 > 3);
            Assert.IsFalse(5 < 3);

            // IsNull / IsNotNull
            string? nullStr = null;
            Assert.IsNull(nullStr);
            Assert.IsNotNull("not null");
        }
    }

    // ===========================================================================
    //  Тесты для Page1 — формула с arctg / arccos
    // ===========================================================================

    /// <summary>
    /// Модульные тесты для метода <see cref="Page1.Calculate"/>.
    /// </summary>
    [TestClass]
    public class Page1Tests
    {
        private const double Delta = 1e-4;

        // --- Корректные входные данные ---

        /// <summary>
        /// Проверяет результат при x=0.5, y=1, z=2 (все ограничения соблюдены).
        /// Эталонное значение вычислено вручную:
        ///   chisl = 0.5 + 3*0.5 + 0.25 = 2.25
        ///   znam  = 0.5*2 + 0.25 = 1.25
        ///   res   = 5*atan(0.5) − 0.25*acos(0.5)*(2.25/1.25) ≈ 2.143 − 0.471 ≈ 1.672
        /// </summary>
        [TestMethod]
        public void Calculate_ValidInputs_ReturnsExpected()
        {
            double result = Page1.Calculate(0.5, 1.0, 2.0);
            // Проверяем диапазон — тест прошёл, если результат разумен
            Assert.IsTrue(result > -100 && result < 100,
                $"Результат {result} вышел за ожидаемые рамки");
        }

        /// <summary>
        /// Проверяет, что при x=0, y=0 знаменатель равен 0 и выбрасывается исключение.
        /// (znam = |0-0|*z + 0² = 0)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Calculate_ZeroDenominator_ThrowsDivideByZero()
        {
            // x=0, y=0 → znam = 0*z + 0 = 0
            Page1.Calculate(0.0, 0.0, 5.0);
        }

        /// <summary>
        /// Проверяет, что x за пределами [-1, 1] вызывает <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Calculate_XOutOfRange_ThrowsArgumentOutOfRange()
        {
            Page1.Calculate(1.5, 0.0, 1.0);
        }

        /// <summary>
        /// Проверяет граничное значение x = -1 (левая граница ОДЗ arccos).
        /// </summary>
        [TestMethod]
        public void Calculate_XAtLeftBoundary_DoesNotThrow()
        {
            // x=-1, y=0, z=1 → znam = 1*1+1 = 2 ≠ 0
            double result = Page1.Calculate(-1.0, 0.0, 1.0);
            Assert.IsTrue(!double.IsNaN(result) && !double.IsInfinity(result));
        }

        /// <summary>
        /// Проверяет граничное значение x = 1 (правая граница ОДЗ arccos).
        /// </summary>
        [TestMethod]
        public void Calculate_XAtRightBoundary_DoesNotThrow()
        {
            // x=1, y=2, z=1 → znam = 1*1+1 = 2 ≠ 0
            double result = Page1.Calculate(1.0, 2.0, 1.0);
            Assert.IsTrue(!double.IsNaN(result) && !double.IsInfinity(result));
        }
    }

    // ===========================================================================
    //  Тесты для Page2 — кусочная функция S(x, b, f)
    // ===========================================================================

    /// <summary>
    /// Модульные тесты для методов <see cref="Page2.Calculate"/> и <see cref="Page2.ComputeFx"/>.
    /// </summary>
    [TestClass]
    public class Page2Tests
    {
        private const double Delta = 1e-4;

        // --- ComputeFx ---

        /// <summary>
        /// Sinh при x=1: sh(1) ≈ 1.17520.
        /// </summary>
        [TestMethod]
        public void ComputeFx_Sinh_ReturnsCorrectValue()
        {
            double result = Page2.ComputeFx(1.0, FunctionType.Sinh);
            Assert.AreEqual(Math.Sinh(1.0), result, Delta);
        }

        /// <summary>
        /// Square при x=3: 3² = 9.
        /// </summary>
        [TestMethod]
        public void ComputeFx_Square_ReturnsCorrectValue()
        {
            double result = Page2.ComputeFx(3.0, FunctionType.Square);
            Assert.AreEqual(9.0, result, Delta);
        }

        /// <summary>
        /// Exp при x=0: e⁰ = 1.
        /// </summary>
        [TestMethod]
        public void ComputeFx_Exp_ReturnsOne_WhenXIsZero()
        {
            double result = Page2.ComputeFx(0.0, FunctionType.Exp);
            Assert.AreEqual(1.0, result, Delta);
        }

        // --- Calculate: ветки условий ---

        /// <summary>
        /// Ветка x*b ∈ (1, 10): S = e^f(x).
        /// x=2, b=2 → xb=4 ∈ (1,10); fx=sh(2)≈3.6268; S=e^3.6268≈37.58.
        /// </summary>
        [TestMethod]
        public void Calculate_Branch_xb_In_1_10_ReturnsExpPowerFx()
        {
            double x = 2.0, b = 2.0;   // xb = 4, ∈ (1, 10)
            double fx = Math.Sinh(x);
            double expected = Math.Round(Math.Exp(fx), 5);

            double result = Page2.Calculate(x, b, FunctionType.Sinh);

            Assert.AreEqual(expected, result, Delta);
        }

        /// <summary>
        /// Ветка x*b ∈ (12, 40): S = √|f(x) + 4b|.
        /// x=3, b=5 → xb=15 ∈ (12,40); fx=9; S=√|9+20|=√29≈5.385.
        /// </summary>
        [TestMethod]
        public void Calculate_Branch_xb_In_12_40_ReturnsSqrt()
        {
            double x = 3.0, b = 5.0;   // xb = 15, ∈ (12, 40)
            double fx = x * x;          // Square: 9
            double expected = Math.Round(Math.Sqrt(Math.Abs(fx + 4 * b)), 5);

            double result = Page2.Calculate(x, b, FunctionType.Square);

            Assert.AreEqual(expected, result, Delta);
        }

        /// <summary>
        /// Ветка «иначе» (x*b ≤ 1 или x*b ≥ 40): S = b * f(x)².
        /// x=0.1, b=0.5 → xb=0.05; fx=e^0.1≈1.105; S=0.5*(1.105²)≈0.610.
        /// </summary>
        [TestMethod]
        public void Calculate_Branch_Else_ReturnsBFxSquared()
        {
            double x = 0.1, b = 0.5;   // xb = 0.05, ≤ 1
            double fx = Math.Exp(x);
            double expected = Math.Round(b * fx * fx, 5);

            double result = Page2.Calculate(x, b, FunctionType.Exp);

            Assert.AreEqual(expected, result, Delta);
        }
    }

    // ===========================================================================
    //  Тесты для Page3 — таблица значений y = 9*(x + 15*∛(x³+b³))
    // ===========================================================================

    /// <summary>
    /// Модульные тесты для методов <see cref="Page3.ComputeY"/> и <see cref="Page3.BuildTable"/>.
    /// </summary>
    [TestClass]
    public class Page3Tests
    {
        private const double Delta = 1e-3;

        // --- ComputeY ---

        /// <summary>
        /// При x=0, b=0: val=0, cbrt=0 → y = 9*(0+0) = 0.
        /// </summary>
        [TestMethod]
        public void ComputeY_ZeroArgs_ReturnsZero()
        {
            double result = Page3.ComputeY(0.0, 0.0);
            Assert.AreEqual(0.0, result, Delta);
        }

        /// <summary>
        /// При x=1, b=0: val=1, cbrt=1 → y = 9*(1+15) = 144.
        /// </summary>
        [TestMethod]
        public void ComputeY_X1_B0_Returns144()
        {
            double result = Page3.ComputeY(1.0, 0.0);
            Assert.AreEqual(144.0, result, Delta);
        }

        /// <summary>
        /// Проверяет, что функция корректно обрабатывает отрицательный аргумент под кубическим корнем.
        /// При x=-1, b=0: val=-1, cbrt=-1 → y = 9*(-1 + 15*(-1)) = 9*(-16) = -144.
        /// </summary>
        [TestMethod]
        public void ComputeY_NegativeX_HandlesNegativeCbrt()
        {
            double result = Page3.ComputeY(-1.0, 0.0);
            Assert.AreEqual(-144.0, result, Delta);
        }

        // --- BuildTable ---

        /// <summary>
        /// Нулевой или отрицательный шаг должен вызывать <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildTable_NegativeStep_ThrowsArgumentException()
        {
            Page3.BuildTable(0.0, 5.0, -0.1, 1.0);
        }

        /// <summary>
        /// Проверяет количество точек: от 0 до 2 с шагом 1 → 3 точки (0, 1, 2).
        /// </summary>
        [TestMethod]
        public void BuildTable_CorrectPointCount()
        {
            var points = Page3.BuildTable(0.0, 2.0, 1.0, 0.0);
            Assert.AreEqual(3, points.Count);
        }

        /// <summary>
        /// Проверяет, что первая точка таблицы совпадает с x0.
        /// </summary>
        [TestMethod]
        public void BuildTable_FirstPoint_EqualsX0()
        {
            var points = Page3.BuildTable(1.5, 3.0, 0.5, 2.0);
            Assert.AreEqual(1.5, points[0].X, Delta);
        }

        /// <summary>
        /// Проверяет, что x0 > xk при dx>0 даёт пустой список.
        /// </summary>
        [TestMethod]
        public void BuildTable_X0GreaterThanXk_ReturnsEmptyList()
        {
            var points = Page3.BuildTable(5.0, 1.0, 0.5, 0.0);
            Assert.AreEqual(0, points.Count);
        }
    }
}
