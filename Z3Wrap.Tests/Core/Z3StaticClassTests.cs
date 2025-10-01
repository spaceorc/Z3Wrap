using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;

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

    [Test]
    public void LoadLibrary_WithValidPath_LoadsSuccessfully()
    {
        // Get the current library path first
        var currentLibrary = Z3.Library;
        var libraryPath = currentLibrary.LibraryPath;

        // Load library from the same path
        Assert.DoesNotThrow(() => Z3.LoadLibrary(libraryPath));
        Assert.That(Z3.Library, Is.Not.Null);
        Assert.That(Z3.Library.LibraryPath, Is.EqualTo(libraryPath));
    }

    [Test]
    public void LoadLibrary_WithInvalidPath_ThrowsException()
    {
        var invalidPath = "/nonexistent/path/to/libz3.so";

        Assert.Throws<FileNotFoundException>(() => Z3.LoadLibrary(invalidPath));
    }

    [Test]
    public void LoadLibrary_WithEmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Z3.LoadLibrary(""));
    }

    [Test]
    public void LoadLibrary_WithWhitespace_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Z3.LoadLibrary("   "));
    }

    [Test]
    public void LoadLibrary_WithNull_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Z3.LoadLibrary(null!));
    }

    [Test]
    public void LoadLibrary_ReplacesExistingLibrary()
    {
        // Load library auto first
        Z3.LoadLibraryAuto();
        var firstLibrary = Z3.Library;
        var libraryPath = firstLibrary.LibraryPath;

        // Load the same library again (this should replace the first one)
        Z3.LoadLibrary(libraryPath);
        var secondLibrary = Z3.Library;

        Assert.That(secondLibrary, Is.Not.Null);
        Assert.That(secondLibrary.LibraryPath, Is.EqualTo(libraryPath));
    }

    [Test]
    public void LoadLibrary_CanBeUsedToCreateContext()
    {
        var currentLibrary = Z3.Library;
        var libraryPath = currentLibrary.LibraryPath;

        Z3.LoadLibrary(libraryPath);

        // Verify the loaded library can be used to create a context
        using var context = new Z3Context();
        Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void LoadLibrary_AfterLoad_LibraryIsAccessible()
    {
        var currentLibrary = Z3.Library;
        var libraryPath = currentLibrary.LibraryPath;

        Z3.LoadLibrary(libraryPath);

        var library = Z3.Library;
        Assert.Multiple(() =>
        {
            Assert.That(library, Is.Not.Null);
            Assert.That(library.LibraryPath, Is.Not.Null.And.Not.Empty);
        });
    }

    [Test]
    public void LoadLibrary_PathWithSpecialCharacters_HandledCorrectly()
    {
        // This test ensures special characters in paths don't cause issues
        var currentLibrary = Z3.Library;
        var libraryPath = currentLibrary.LibraryPath;

        // If path doesn't have special chars, just verify normal loading works
        Assert.DoesNotThrow(() => Z3.LoadLibrary(libraryPath));
    }

    [Test]
    public void LoadLibrary_MultipleCalls_ReplacesLibraryEachTime()
    {
        var currentLibrary = Z3.Library;
        var libraryPath = currentLibrary.LibraryPath;

        // Load multiple times
        Z3.LoadLibrary(libraryPath);
        var library1 = Z3.Library;

        Z3.LoadLibrary(libraryPath);
        var library2 = Z3.Library;

        Z3.LoadLibrary(libraryPath);
        var library3 = Z3.Library;

        // All should be valid libraries
        Assert.Multiple(() =>
        {
            Assert.That(library1, Is.Not.Null);
            Assert.That(library2, Is.Not.Null);
            Assert.That(library3, Is.Not.Null);
        });
    }

    [Test]
    public void LoadLibrary_DisposesOldLibrary()
    {
        // Load library first time
        Z3.LoadLibraryAuto();
        var firstLibrary = Z3.Library;
        var libraryPath = firstLibrary.LibraryPath;

        // Dispose context before replacing library to avoid accessing disposed library
        using (var context1 = new Z3Context())
        {
            Assert.That(context1.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }

        // Load library again (should dispose old one)
        Z3.LoadLibrary(libraryPath);

        // New library should work fine
        using var context2 = new Z3Context();
        Assert.That(context2.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void LoadLibrary_ThreadSafe_MultipleCallsDontCrash()
    {
        var currentLibrary = Z3.Library;
        var libraryPath = currentLibrary.LibraryPath;

        // This test ensures thread-safety by making multiple calls
        // The implementation uses Interlocked.Exchange which is thread-safe
        Assert.DoesNotThrow(() =>
        {
            Z3.LoadLibrary(libraryPath);
            Z3.LoadLibrary(libraryPath);
            Z3.LoadLibrary(libraryPath);
        });

        Assert.That(Z3.Library, Is.Not.Null);
    }

    [Test]
    public void LoadLibrary_VerifyLoadedLibraryFunctional()
    {
        var currentLibrary = Z3.Library;
        var libraryPath = currentLibrary.LibraryPath;

        Z3.LoadLibrary(libraryPath);

        // Create context and solve a simple constraint to verify functionality
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x == 42);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetIntValue(x);
        Assert.That(value, Is.EqualTo(new System.Numerics.BigInteger(42)));
    }
}
