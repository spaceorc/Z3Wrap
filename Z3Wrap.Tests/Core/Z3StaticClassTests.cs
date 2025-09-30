using Spaceorc.Z3Wrap.Core;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3StaticClassTests
{
    [Test]
    public void Library_AutoInitializes()
    {
        var library = Z3.Library;

        Assert.That(library, Is.Not.Null);
        Assert.That(library.LibraryPath, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void LoadLibraryAuto_SetsLibrary()
    {
        Z3.LoadLibraryAuto();

        var library = Z3.Library;

        Assert.That(library, Is.Not.Null);
        Assert.That(library.LibraryPath, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Library_CanBeSet()
    {
        var customLibrary = Z3Library.LoadAuto();

        Assert.DoesNotThrow(() => Z3.Library = customLibrary);
        Assert.That(Z3.Library, Is.Not.Null);
    }
}
