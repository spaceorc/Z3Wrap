using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Core;

[TestFixture]
public class Z3ErrorHandlingTests
{
    [Test]
    public void ForAll_InvalidBoundVariable_ThrowsZ3Exception()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var body = x > 0;

        // Passing a constant instead of bound variable should trigger Z3_INVALID_ARG
        var ex = Assert.Throws<Z3Exception>(() => context.ForAll(context.Int(10), body));

        Assert.That(ex.ErrorCode, Is.EqualTo(Z3ErrorCode.InvalidArgument));
        Assert.That(ex.Message, Contains.Substring("invalid argument"));
    }

    [Test]
    public void Exists_InvalidBoundVariable_ThrowsZ3Exception()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var body = x > 0;

        // Passing a constant instead of bound variable should trigger Z3_INVALID_ARG
        var ex = Assert.Throws<Z3Exception>(() => context.Exists(context.Int(42), body));

        Assert.That(ex.ErrorCode, Is.EqualTo(Z3ErrorCode.InvalidArgument));
        Assert.That(ex.Message, Contains.Substring("invalid argument"));
    }

    [Test]
    public void ForAll_TwoVariables_InvalidFirstVariable_ThrowsZ3Exception()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var y = context.IntConst("y");
        var body = y > 0;

        // Invalid first bound variable
        var ex = Assert.Throws<Z3Exception>(() => context.ForAll(context.Int(1), y, body));

        Assert.That(ex.ErrorCode, Is.EqualTo(Z3ErrorCode.InvalidArgument));
        Assert.That(ex.Message, Contains.Substring("invalid argument"));
    }

    [Test]
    public void Exists_TwoVariables_InvalidSecondVariable_ThrowsZ3Exception()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var body = x > 0;

        // Invalid second bound variable
        var ex = Assert.Throws<Z3Exception>(() => context.Exists(x, context.Real(3.14m), body));

        Assert.That(ex.ErrorCode, Is.EqualTo(Z3ErrorCode.InvalidArgument));
        Assert.That(ex.Message, Contains.Substring("invalid argument"));
    }

    [Test]
    public void ForAll_ThreeVariables_InvalidMiddleVariable_ThrowsZ3Exception()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var z = context.IntConst("z");
        var body = x + z > 0;

        // Invalid middle bound variable (using constant instead of variable)
        var ex = Assert.Throws<Z3Exception>(() => context.ForAll(x, context.Int(99), z, body));

        Assert.That(ex.ErrorCode, Is.EqualTo(Z3ErrorCode.InvalidArgument));
        Assert.That(ex.Message, Contains.Substring("invalid argument"));
    }

    [Test]
    public void SafeNativeMethods_NoError_DoesNotThrow()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");

        // This should not throw since it's a valid operation
        Assert.DoesNotThrow(() => context.Bool(true));
    }

    [Test]
    public void SafeNativeMethods_AfterValidOperation_DoesNotThrow()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var body = x > 0;

        // Valid quantifier creation should not throw
        Assert.DoesNotThrow(() =>
        {
            var forall = context.ForAll(x, body);
            Assert.That(forall, Is.Not.Null);
        });
    }

    [Test]
    public void Z3Exception_Properties_AreSetCorrectly()
    {
        var errorCode = Z3ErrorCode.InvalidArgument;
        var message = "Test error message";

        var exception = new Z3Exception(errorCode, message);

        Assert.That(exception.ErrorCode, Is.EqualTo(errorCode));
        Assert.That(exception.Message, Is.EqualTo("Z3 Error (InvalidArgument): Test error message"));
    }

    [Test]
    public void Z3Exception_InheritsFromException()
    {
        var exception = new Z3Exception(Z3ErrorCode.Ok, "test");

        Assert.That(exception, Is.InstanceOf<Exception>());
    }

    [Test]
    public void Z3ErrorCode_HasExpectedValues()
    {
        // Test that key error codes have expected integer values
        Assert.That((int)Z3ErrorCode.Ok, Is.EqualTo(0));
        Assert.That((int)Z3ErrorCode.InvalidArgument, Is.EqualTo(3)); // Based on Z3 source
    }

    [Test]
    public void ErrorHandling_DoesNotCrashProcess()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var body = x > 0;

        // This test verifies that we get a managed exception instead of a process crash
        // Multiple invalid calls to ensure robustness
        for (int i = 0; i < 3; i++)
        {
            Assert.Throws<Z3Exception>(() => context.ForAll(context.Int(i), body));
        }

        // Process should still be alive and context should still work
        var validQuantifier = context.ForAll(x, body);
        Assert.That(validQuantifier, Is.Not.Null);
    }
}
