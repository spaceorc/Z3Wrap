using Spaceorc.Z3Wrap;
namespace Z3Wrap.Tests.Unit.Core;

[TestFixture]
public class Z3ContextTests
{

    [Test]
    public void Constructor_DefaultConfig_CreatesAndDisposesSuccessfully()
    {
        using var context = new Z3Context();
        Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Constructor_WithParameters_CreatesSuccessfully()
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
    public void SetParameter_ValidParameter_DoesNotThrow()
    {
        using var context = new Z3Context();
        Assert.DoesNotThrow(() => context.SetParameter("timeout", "1000"));
    }

    [Test]
    public void Handle_DisposedContext_ThrowsObjectDisposedException()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = context.Handle);
        Assert.Throws<ObjectDisposedException>(() => context.SetParameter("test", "value"));
    }

    [Test]
    public void Dispose_MultipleCalls_DoesNotThrow()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.DoesNotThrow(() => context.Dispose());
    }
}