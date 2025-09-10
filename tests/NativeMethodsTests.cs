using z3lib;

namespace tests;

public class NativeMethodsTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        NativeMethods.LoadLibrary("/opt/homebrew/opt/z3/lib/libz3.dylib");
    }

    [Test]
    public void CanCreateAndDeleteConfig()
    {
        var config = NativeMethods.Z3MkConfig();
        Assert.That(config, Is.Not.EqualTo(IntPtr.Zero));

        NativeMethods.Z3DelConfig(config);
        Assert.Pass();
    }

    [Test]
    public void CanCreateAndDeleteContext()
    {
        var config = NativeMethods.Z3MkConfig();
        Assert.That(config, Is.Not.EqualTo(IntPtr.Zero));

        var context = NativeMethods.Z3MkContext(config);
        Assert.That(context, Is.Not.EqualTo(IntPtr.Zero));

        NativeMethods.Z3DelContext(context);
        NativeMethods.Z3DelConfig(config);
        Assert.Pass();
    }

    [Test]
    public void CanCreateAndDeleteContextRc()
    {
        var config = NativeMethods.Z3MkConfig();
        Assert.That(config, Is.Not.EqualTo(IntPtr.Zero));

        var context = NativeMethods.Z3MkContextRc(config);
        Assert.That(context, Is.Not.EqualTo(IntPtr.Zero));

        NativeMethods.Z3DelContext(context);
        NativeMethods.Z3DelConfig(config);
        Assert.Pass();
    }
}