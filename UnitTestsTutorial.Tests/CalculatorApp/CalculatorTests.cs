using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestsTutorial.CalculatorApp;
using Xunit;

namespace UnitTestsTutorial.Tests.CalculatorApp
{
    [Trait("Calculator", "Divide")]
    public class CalculatorTests
    {
        [Theory]
        [InlineData(15, 3, 5)]
        [InlineData(100000, 2, 50000)]
        public void Should_Valid_Divide_Numbers(int number1, int number2, int expectedResult)
        {
            // arrange
            var sut = new Calculator();

            // act
            var result = sut.Divide(number1, number2);

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Should_Throw_When_Divide_By_Zero()
        {
            // arrange
            var number1 = 15;
            var number2 = 0;

            var sut = new Calculator();

            // act assert
            Assert.Throws<DivideByZeroException>(() => sut.Divide(number1, number2));
        }
    }
}
