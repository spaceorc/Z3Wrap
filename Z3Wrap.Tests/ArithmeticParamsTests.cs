using NUnit.Framework;
using Z3Wrap;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests
{
    [TestFixture]
    public class ArithmeticParamsTests
    {
        private Z3Context context;

        [SetUp]
        public void SetUp()
        {
            context = new Z3Context();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public void IntAdd_OneOperand_ReturnsSameOperand()
        {
            // Arrange
            var x = context.IntConst("x");

            // Act
            var result = context.Add(x);

            // Assert - should be equivalent to x
            using var solver = context.CreateSolver();
            solver.Assert(context.Neq(result, x)); // They should be the same, so this should be unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void IntAdd_TwoOperands_WorksLikeOriginal()
        {
            // Arrange
            var x = context.IntConst("x");
            var y = context.IntConst("y");

            // Act
            var result = context.Add(x, y);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(context.Eq(x, context.Int(3)));
            solver.Assert(context.Eq(y, context.Int(5)));
            solver.Assert(context.Eq(result, context.Int(8)));
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        }

        [Test]
        public void IntAdd_ThreeOperands_AddsAll()
        {
            // Arrange
            var x = context.IntConst("x");
            var y = context.IntConst("y");
            var z = context.IntConst("z");

            // Act
            var result = context.Add(x, y, z);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(context.Eq(x, context.Int(1)));
            solver.Assert(context.Eq(y, context.Int(2)));
            solver.Assert(context.Eq(z, context.Int(3)));
            solver.Assert(context.Eq(result, context.Int(6)));
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        }

        [Test]
        public void IntMul_OneOperand_ReturnsSameOperand()
        {
            // Arrange
            var x = context.IntConst("x");

            // Act
            var result = context.Mul(x);

            // Assert - should be equivalent to x
            using var solver = context.CreateSolver();
            solver.Assert(context.Neq(result, x)); // They should be the same, so this should be unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void IntMul_ThreeOperands_MultipliesAll()
        {
            // Arrange
            var x = context.IntConst("x");
            var y = context.IntConst("y");
            var z = context.IntConst("z");

            // Act
            var result = context.Mul(x, y, z);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(context.Eq(x, context.Int(2)));
            solver.Assert(context.Eq(y, context.Int(3)));
            solver.Assert(context.Eq(z, context.Int(4)));
            solver.Assert(context.Eq(result, context.Int(24)));
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        }

        [Test]
        public void IntSub_ThreeOperands_SubtractsLeftToRight()
        {
            // Arrange
            var x = context.IntConst("x");
            var y = context.IntConst("y");
            var z = context.IntConst("z");

            // Act - should be ((x - y) - z)
            var result = context.Sub(x, y, z);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(context.Eq(x, context.Int(10)));
            solver.Assert(context.Eq(y, context.Int(3)));
            solver.Assert(context.Eq(z, context.Int(2)));
            solver.Assert(context.Eq(result, context.Int(5))); // 10 - 3 - 2 = 5
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        }

        [Test]
        public void RealAdd_ThreeOperands_AddsAll()
        {
            // Arrange
            var x = context.RealConst("x");
            var y = context.RealConst("y");
            var z = context.RealConst("z");

            // Act
            var result = context.Add(x, y, z);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(context.Eq(x, context.Real(1.5m)));
            solver.Assert(context.Eq(y, context.Real(2.3m)));
            solver.Assert(context.Eq(z, context.Real(0.2m)));
            solver.Assert(context.Eq(result, context.Real(4.0m)));
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        }

        [Test]
        public void RealMul_ThreeOperands_MultipliesAll()
        {
            // Arrange
            var x = context.RealConst("x");
            var y = context.RealConst("y");
            var z = context.RealConst("z");

            // Act
            var result = context.Mul(x, y, z);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(context.Eq(x, context.Real(0.5m)));
            solver.Assert(context.Eq(y, context.Real(2.0m)));
            solver.Assert(context.Eq(z, context.Real(3.0m)));
            solver.Assert(context.Eq(result, context.Real(3.0m)));
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        }
    }
}