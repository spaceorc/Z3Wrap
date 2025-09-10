using z3lib;

namespace tests;

public class Z3ContextTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        NativeMethods.LoadLibrary("/opt/homebrew/opt/z3/lib/libz3.dylib");
    }

    [Test]
    public void CanCreateAndDisposeContext()
    {
        using (var context = new Z3Context())
        {
            Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }

        Assert.Pass();
    }

    [Test]
    public void CanCreateContextWithParameters()
    {
        var parameters = new Dictionary<string, string>
        {
            { "model", "true" },
            { "proof", "false" },
        };

        using (var context = new Z3Context(parameters))
        {
            Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }

        Assert.Pass();
    }

    [Test]
    public void CanSetParameterAfterCreation()
    {
        using (var context = new Z3Context())
        {
            Assert.DoesNotThrow(() => context.SetParameter("timeout", "1000"));
        }

        Assert.Pass();
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