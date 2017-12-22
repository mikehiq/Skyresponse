using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace XUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
        }

        [Fact]
        public void PassingTest()
        {
            Xunit.Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Xunit.Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public void MyFirstTheory(int value)
        {
            Xunit.Assert.True(IsOdd(value));
        }

        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }
    }
}
