namespace z3lib.Tests.CoreTests;

public class Z3ContextTests
{

    [Test]
    public void CanCreateAndDisposeContext()
    {
        using var context = new Z3Context();
        Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanCreateContextWithParameters()
    {
        var parameters = new Dictionary<string, string>
        {
            { "model", "true" },
            { "proof", "false" },
        };

        using var context = new Z3Context(parameters);
        Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanSetParameterAfterCreation()
    {
        using var context = new Z3Context();
        Assert.DoesNotThrow(() => context.SetParameter("timeout", "1000"));
    }

    [Test]
    public void ThrowsWhenAccessingDisposedContext()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = context.Handle);
        Assert.Throws<ObjectDisposedException>(() => context.SetParameter("test", "value"));
    }

    [Test]
    public void CanDisposeMultipleTimes()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.DoesNotThrow(() => context.Dispose());
    }
}