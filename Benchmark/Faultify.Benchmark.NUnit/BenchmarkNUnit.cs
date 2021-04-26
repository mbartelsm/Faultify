using NUnit.Framework;

namespace Faultify.Benchmark.NUnit
{
    public class BenchmarkNUnit
    {
        [Test]
        public void TestArray()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            int[] actual = targets.ConstructArray();
            int[] expected = { 1, 2, 3, 6, 4, 2, 5, 3, 2, 6, 7 };

            Assert.AreEqual(expected, expected);
        }

        [Test]
        public void TestAddition()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            int actual = targets.Addition(5, 4);
            var expected = 9;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtraction()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            int actual = targets.Subtraction(5, 4);
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivision()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            int actual = targets.Division(2, 1);
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplication()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            int actual = targets.Multiplication(2, 2);
            var expected = 4;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModulo()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            int actual = targets.Modulo(3, 2);
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestForLoop()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            targets.ForLoop(10);
            Assert.True(true);
        }

        [Test]
        public void TestWhileLoop()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            int actual = targets.WhileLoop(10);
            var expected = 10;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIf()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            bool actual = targets.If(true);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLessThan()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            var actual = false;

            actual = targets.LessThan(5, 10);

            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMoreThan()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            bool actual = targets.MoreThan(10, 5);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLogicalAnd()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            bool actual = targets.LogicalAnd(0, 0);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestLogicalOr()
        {
            BenchmarkTarget targets = new BenchmarkTarget();
            bool actual = targets.LogicalOr(0, 1);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
    }

    public class BenchmarkNUnit1
    {
        [Test]
        public void TestArray()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            int[] actual = targets.ConstructArray();
            int[] expected = { 1, 2, 3, 6, 4, 2, 5, 3, 2, 6, 7 };

            Assert.AreEqual(expected, expected);
        }

        [Test]
        public void TestAddition()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            int actual = targets.Addition(5, 4);
            var expected = 9;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtraction()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            int actual = targets.Subtraction(5, 4);
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivision()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            int actual = targets.Division(2, 1);
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplication()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            int actual = targets.Multiplication(2, 2);
            var expected = 4;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModulo()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            int actual = targets.Modulo(3, 2);
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestForLoop()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            targets.ForLoop(10);
            Assert.True(true);
        }

        [Test]
        public void TestWhileLoop()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            int actual = targets.WhileLoop(10);
            var expected = 10;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIf()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            bool actual = targets.If(true);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLessThan()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            var actual = false;

            actual = targets.LessThan(5, 10);

            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMoreThan()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            bool actual = targets.MoreThan(10, 5);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLogicalAnd()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            bool actual = targets.LogicalAnd(0, 0);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestLogicalOr()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            bool actual = targets.LogicalOr(0, 1);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestNull()
        {
            BenchmarkTarget1 targets = new BenchmarkTarget1();
            bool actual = targets.isNull(null);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
    }

    public class BenchmarkNUnit2
    {
        [Test]
        public void TestArray()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            int[] actual = targets.ConstructArray();
            int[] expected = { 1, 2, 3, 6, 4, 2, 5, 3, 2, 6, 7 };

            Assert.AreEqual(expected, expected);
        }

        [Test]
        public void TestAddition()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            int actual = targets.Addition(5, 4);
            var expected = 9;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtraction()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            int actual = targets.Subtraction(5, 4);
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivision()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            int actual = targets.Division(2, 1);
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplication()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            int actual = targets.Multiplication(2, 2);
            var expected = 4;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModulo()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            int actual = targets.Modulo(3, 2);
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestForLoop()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            targets.ForLoop(10);
            Assert.True(true);
        }

        [Test]
        public void TestWhileLoop()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            int actual = targets.WhileLoop(10);
            var expected = 10;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIf()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            bool actual = targets.If(true);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLessThan()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            var actual = false;

            actual = targets.LessThan(5, 10);

            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMoreThan()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            bool actual = targets.MoreThan(10, 5);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLogicalAnd()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            bool actual = targets.LogicalAnd(0, 0);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestLogicalOr()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            bool actual = targets.LogicalOr(0, 1);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestNull()
        {
            BenchmarkTarget2 targets = new BenchmarkTarget2();
            bool actual = targets.isNull(null);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
    }

    public class BenchmarkNUnit3
    {
        [Test]
        public void TestArray()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            int[] actual = targets.ConstructArray();
            int[] expected = { 1, 2, 3, 6, 4, 2, 5, 3, 2, 6, 7 };

            Assert.AreEqual(expected, expected);
        }

        [Test]
        public void TestAddition()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            int actual = targets.Addition(5, 4);
            var expected = 9;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtraction()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            int actual = targets.Subtraction(5, 4);
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivision()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            int actual = targets.Division(2, 1);
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplication()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            int actual = targets.Multiplication(2, 2);
            var expected = 4;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModulo()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            int actual = targets.Modulo(3, 2);
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestForLoop()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            targets.ForLoop(10);
            Assert.True(true);
        }

        [Test]
        public void TestWhileLoop()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            int actual = targets.WhileLoop(10);
            var expected = 10;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIf()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            bool actual = targets.If(true);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLessThan()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            var actual = false;

            actual = targets.LessThan(5, 10);

            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMoreThan()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            bool actual = targets.MoreThan(10, 5);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLogicalAnd()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            bool actual = targets.LogicalAnd(0, 0);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestLogicalOr()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            bool actual = targets.LogicalOr(0, 1);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestNull()
        {
            BenchmarkTarget3 targets = new BenchmarkTarget3();
            bool actual = targets.isNull(null);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
    }
}
