using Spaceorc.Z3Wrap.Core;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3LibraryTests
{
    [Test]
    public void LoadAuto_CreatesLibrary()
    {
        using var library = Z3Library.LoadAuto();

        Assert.That(library, Is.Not.Null);
        Assert.That(library.LibraryPath, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void LibraryPath_ReturnsValidPath()
    {
        using var library = Z3Library.LoadAuto();

        var path = library.LibraryPath;

        Assert.That(path, Does.Contain("z3").IgnoreCase);
    }

    [Test]
    public void Dispose_MultipleCalls_DoesNotThrow()
    {
        var library = Z3Library.LoadAuto();
        library.Dispose();

        Assert.DoesNotThrow(() => library.Dispose());
    }

    [Test]
    public void MkConfig_ReturnsNonZeroHandle()
    {
        using var library = Z3Library.LoadAuto();

        var configHandle = library.MkConfig();

        Assert.That(configHandle, Is.Not.EqualTo(IntPtr.Zero));

        library.DelConfig(configHandle);
    }

    [Test]
    public void MkContextRc_ReturnsNonZeroHandle()
    {
        using var library = Z3Library.LoadAuto();
        var configHandle = library.MkConfig();

        var contextHandle = library.MkContextRc(configHandle);

        Assert.That(contextHandle, Is.Not.EqualTo(IntPtr.Zero));

        library.DelContext(contextHandle);
        library.DelConfig(configHandle);
    }

    [Test]
    public void MkBoolSort_ReturnsNonZeroHandle()
    {
        using var library = Z3Library.LoadAuto();
        var configHandle = library.MkConfig();
        var contextHandle = library.MkContextRc(configHandle);

        var sortHandle = library.MkBoolSort(contextHandle);

        Assert.That(sortHandle, Is.Not.EqualTo(IntPtr.Zero));

        library.DelContext(contextHandle);
        library.DelConfig(configHandle);
    }
}
