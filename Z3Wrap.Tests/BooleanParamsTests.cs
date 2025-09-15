using NUnit.Framework;
using Z3Wrap;

namespace Z3Wrap.Tests
{
    [TestFixture]
    public class BooleanParamsTests
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
        public void And_ZeroOperands_ReturnsTrue()
        {
            // Act
            var result = context.And();

            // Assert - should be equivalent to true
            using var solver = context.CreateSolver();
            solver.Assert(context.Neq(result, context.True())); // They should be the same, so this should be unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void Or_ZeroOperands_ReturnsFalse()
        {
            // Act
            var result = context.Or();

            // Assert - should be equivalent to false
            using var solver = context.CreateSolver();
            solver.Assert(context.Neq(result, context.False())); // They should be the same, so this should be unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void And_OneOperand_ReturnsSameOperand()
        {
            // Arrange
            var x = context.BoolConst("x");

            // Act
            var result = context.And(x);

            // Assert - should be equivalent to x
            using var solver = context.CreateSolver();
            solver.Assert(context.Neq(result, x)); // They should be the same, so this should be unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void Or_OneOperand_ReturnsSameOperand()
        {
            // Arrange
            var x = context.BoolConst("x");

            // Act
            var result = context.Or(x);

            // Assert - should be equivalent to x
            using var solver = context.CreateSolver();
            solver.Assert(context.Neq(result, x)); // They should be the same, so this should be unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void And_TwoOperands_WorksLikeOriginal()
        {
            // Arrange
            var x = context.BoolConst("x");
            var y = context.BoolConst("y");

            // Act
            var result = context.And(x, y);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(result);
            solver.Assert(context.Not(x));  // This should make the result unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void Or_TwoOperands_WorksLikeOriginal()
        {
            // Arrange
            var x = context.BoolConst("x");
            var y = context.BoolConst("y");

            // Act
            var result = context.Or(x, y);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(result);
            solver.Assert(context.Not(x));
            solver.Assert(context.Not(y));  // Both false should make OR unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void And_ThreeOperands_RequiresAllTrue()
        {
            // Arrange
            var x = context.BoolConst("x");
            var y = context.BoolConst("y");
            var z = context.BoolConst("z");

            // Act
            var result = context.And(x, y, z);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(result);
            solver.Assert(x);
            solver.Assert(y);
            solver.Assert(context.Not(z));  // z false should make AND unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void Or_ThreeOperands_RequiresAtLeastOneTrue()
        {
            // Arrange
            var x = context.BoolConst("x");
            var y = context.BoolConst("y");
            var z = context.BoolConst("z");

            // Act
            var result = context.Or(x, y, z);

            // Assert
            using var solver = context.CreateSolver();
            solver.Assert(result);
            solver.Assert(context.Not(x));
            solver.Assert(context.Not(y));
            solver.Assert(context.Not(z));  // All false should make OR unsatisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void And_ManyOperands_WorksCorrectly()
        {
            // Arrange
            var operands = new[] {
                context.BoolConst("a"),
                context.BoolConst("b"),
                context.BoolConst("c"),
                context.BoolConst("d"),
                context.BoolConst("e")
            };

            // Act
            var result = context.And(operands);

            // Assert - all must be true for AND to be true
            using var solver = context.CreateSolver();
            solver.Assert(result);
            foreach (var op in operands)
                solver.Assert(op);
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

            // Now make one false - should be unsatisfiable
            solver.Assert(context.Not(operands[2]));
            status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }

        [Test]
        public void Or_ManyOperands_WorksCorrectly()
        {
            // Arrange
            var operands = new[] {
                context.BoolConst("a"),
                context.BoolConst("b"),
                context.BoolConst("c"),
                context.BoolConst("d"),
                context.BoolConst("e")
            };

            // Act
            var result = context.Or(operands);

            // Assert - only one needs to be true for OR to be true
            using var solver = context.CreateSolver();
            solver.Assert(result);
            for (int i = 0; i < operands.Length - 1; i++)
                solver.Assert(context.Not(operands[i]));
            // Leave the last one free - should be satisfiable
            var status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

            // Now make all false - should be unsatisfiable
            solver.Assert(context.Not(operands[operands.Length - 1]));
            status = solver.Check();
            Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
        }
    }
}