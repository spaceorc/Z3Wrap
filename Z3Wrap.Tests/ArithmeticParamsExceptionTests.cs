using NUnit.Framework;
using Z3Wrap;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests
{
    [TestFixture]
    public class ArithmeticParamsExceptionTests
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
        public void IntAdd_ZeroOperands_ThrowsInvalidOperationException()
        {
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => context.Add(new Z3IntExpr[0]));
            Assert.That(ex.Message, Does.Contain("Add requires at least one operand"));
        }

        [Test]
        public void IntSub_ZeroOperands_ThrowsInvalidOperationException()
        {
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => context.Sub(new Z3IntExpr[0]));
            Assert.That(ex.Message, Does.Contain("Sub requires at least one operand"));
        }

        [Test]
        public void IntMul_ZeroOperands_ThrowsInvalidOperationException()
        {
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => context.Mul(new Z3IntExpr[0]));
            Assert.That(ex.Message, Does.Contain("Mul requires at least one operand"));
        }

        [Test]
        public void RealAdd_ZeroOperands_ThrowsInvalidOperationException()
        {
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => context.Add(new Z3RealExpr[0]));
            Assert.That(ex.Message, Does.Contain("Add requires at least one operand"));
        }

        [Test]
        public void RealSub_ZeroOperands_ThrowsInvalidOperationException()
        {
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => context.Sub(new Z3RealExpr[0]));
            Assert.That(ex.Message, Does.Contain("Sub requires at least one operand"));
        }

        [Test]
        public void RealMul_ZeroOperands_ThrowsInvalidOperationException()
        {
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => context.Mul(new Z3RealExpr[0]));
            Assert.That(ex.Message, Does.Contain("Mul requires at least one operand"));
        }

        [Test]
        public void BoolAnd_ZeroOperands_ReturnsTrue()
        {
            // Act - And() with 0 operands should work and return True
            var result = context.And();

            // Assert - should be equivalent to true
            using var solver = context.CreateSolver();
            solver.Assert(context.Neq(result, context.True())); // They should be the same, so this should be unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void BoolOr_ZeroOperands_ReturnsFalse()
        {
            // Act - Or() with 0 operands should work and return False
            var result = context.Or();

            // Assert - should be equivalent to false
            using var solver = context.CreateSolver();
            solver.Assert(context.Neq(result, context.False())); // They should be the same, so this should be unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }
    }
}