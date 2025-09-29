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
    public void Library_CanBeSetAndRetrieved()
    {
        var customLibrary = Z3Library.LoadAuto();

        Z3.Library = customLibrary;
        var retrieved = Z3.Library;

        Assert.That(retrieved, Is.SameAs(customLibrary));
    }

    [Test]
    public void Library_Setter_DisposesOldLibrary()
    {
        var firstLibrary = Z3Library.LoadAuto();
        Z3.Library = firstLibrary;

        var secondLibrary = Z3Library.LoadAuto();
        Z3.Library = secondLibrary;

        // Old library is disposed, new one is active
        Assert.That(Z3.Library, Is.SameAs(secondLibrary));
    }
}
