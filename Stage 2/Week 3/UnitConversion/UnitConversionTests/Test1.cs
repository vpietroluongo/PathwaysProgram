using UnitConversion;

namespace UnitConversionTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void PoundsToKilograms_WithValidValue()
        {
            IConvert toKilogramConverter = new ToKilograms();
            UnitConversionService unitConversionService1 = new UnitConversionService(toKilogramConverter);
            double pounds = 10;
            string unit = "lb";
            double expected = 4.53;

            double actual = unitConversionService1.Convert(pounds, unit);

            Assert.AreEqual(expected, actual, 0.001, "Error converting to kilograms");

        }

        [TestMethod]
        public void GramsToKilograms_WithValidValue()
        {
            IConvert toKilogramConverter = new ToKilograms();
            UnitConversionService unitConversionService1 = new UnitConversionService(toKilogramConverter);
            double grams = 1000;
            string unit = "g";
            double expected = 1;

            double actual = unitConversionService1.Convert(grams, unit);

            Assert.AreEqual(expected, actual, 0.001, "Error converting to kilograms");

        }

        [TestMethod]
        public void OuncesToKilograms_WithValidValue()
        {
            IConvert toKilogramConverter = new ToKilograms();
            UnitConversionService unitConversionService1 = new UnitConversionService(toKilogramConverter);
            double ounces = 22;
            string unit = "oz";
            double expected = 0.6236;

            double actual = unitConversionService1.Convert(ounces, unit);

            Assert.AreEqual(expected, actual, 0.001, "Error converting to kilograms");

        }

        [TestMethod]
        public void PoundsToKilograms_WhenValueLessThanZero_ShouldThrowArgumentOutOfRange()
        {
            IConvert toKilogramsConverter = new ToKilograms();
            UnitConversionService unitConversionService1 = new UnitConversionService(toKilogramsConverter);
            double pounds = -10;
            string unit = "lb";
            
            try
            {
                unitConversionService1.Convert(pounds, unit);
            }
            catch (ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, ((ToKilograms) toKilogramsConverter).ValueLessThanZeroMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown");
        }

        [TestMethod]
        public void PoundsToKilograms_WhenInvalidUnit_ShouldThrowException()
        {
            IConvert toKilogramsConverter = new ToKilograms();
            UnitConversionService unitConversionService1 = new UnitConversionService(toKilogramsConverter);
            double pounds = 10;
            string unit = "pounds";

            try
            {
                unitConversionService1.Convert(pounds, unit);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, ((ToKilograms)toKilogramsConverter).InvalidUnitMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown");
        }



        [TestMethod]
        public void KilogramsToPounds_WithValidValue()
        {
            IConvert toPoundsConverter = new ToPounds();
            UnitConversionService unitConversionService1 = new UnitConversionService(toPoundsConverter);
            double kilograms = 10;
            string unit = "kg";
            double expected = 22.0462;

            double actual = unitConversionService1.Convert(kilograms, unit);

            Assert.AreEqual(expected, actual, 0.001, "Error converting to pounds");

        }

        [TestMethod]
        public void GramsToPounds_WithValidValue()
        {
            IConvert toPoundsConverter = new ToPounds();
            UnitConversionService unitConversionService1 = new UnitConversionService(toPoundsConverter);
            double grams = 1000;
            string unit = "g";
            double expected = 2.20462;

            double actual = unitConversionService1.Convert(grams, unit);

            Assert.AreEqual(expected, actual, 0.001, "Error converting to pounds");

        }

        [TestMethod]
        public void OuncesToPounds_WithValidValue()
        {
            IConvert toPoundsConverter = new ToPounds();
            UnitConversionService unitConversionService1 = new UnitConversionService(toPoundsConverter);
            double ounces = 22;
            string unit = "oz";
            double expected = 1.375;

            double actual = unitConversionService1.Convert(ounces, unit);

            Assert.AreEqual(expected, actual, 0.001, "Error converting to pounds");

        }

        [TestMethod]
        public void KilogramsToPounds_WhenValueLessThanZero_ShouldThrowArgumentOutOfRange()
        {
            IConvert toPoundsConverter = new ToPounds();
            UnitConversionService unitConversionService1 = new UnitConversionService(toPoundsConverter);
            double kilograms = -10;
            string unit = "kg";

            try
            {
                unitConversionService1.Convert(kilograms, unit);
            }
            catch (ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, ((ToPounds)toPoundsConverter).ValueLessThanZeroMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown");
        }

        [TestMethod]
        public void KilogramsToPounds_WhenInvalidUnit_ShouldThrowException()
        {
            IConvert toPoundsConverter = new ToPounds();
            UnitConversionService unitConversionService1 = new UnitConversionService(toPoundsConverter);
            double kilograms = 10;
            string unit = "kilograms";

            try
            {
                unitConversionService1.Convert(kilograms, unit);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, ((ToPounds)toPoundsConverter).InvalidUnitMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown");
        }


        public void FahrenheitToCelsius_WithValidValue()
        {
            IConvert toCelsiusConverter = new ToCelsius();
            UnitConversionService unitConversionService1 = new UnitConversionService(toCelsiusConverter);
            double degreesF = 22;
            string unit = "f";
            double expected = -5.5556;

            double actual = unitConversionService1.Convert(degreesF, unit);

            Assert.AreEqual(expected, actual, 0.001, "Error converting to Celsius");

        }

        public void KelvinToCelsius_WithValidValue()
        {
            IConvert toCelsiusConverter = new ToCelsius();
            UnitConversionService unitConversionService1 = new UnitConversionService(toCelsiusConverter);
            double degreesK = 300;
            string unit = "k";
            double expected = 26.28;

            double actual = unitConversionService1.Convert(degreesK, unit);

            Assert.AreEqual(expected, actual, 0.001, "Error converting to Celsius");

        }

        [TestMethod]
        public void KelvinToCelsius_WhenInvalidUnit_ShouldThrowException()
        {
            IConvert toCelsiusConverter = new ToCelsius();
            UnitConversionService unitConversionService1 = new UnitConversionService(toCelsiusConverter);
            double degreesK = 10;
            string unit = "kelvin";

            try
            {
                unitConversionService1.Convert(degreesK, unit);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, ((ToCelsius)toCelsiusConverter).InvalidUnitMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown");
        }
    }
}
