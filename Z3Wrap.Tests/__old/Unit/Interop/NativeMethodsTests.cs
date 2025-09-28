using Spaceorc.Z3Wrap.Core;

namespace Z3Wrap.Tests.__old.Unit.Interop;

[TestFixture]
public class Z3LoadingTests
{
    [Test]
    public void LoadLibrary_EmptyPath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Z3.LoadLibrary(""));
    }

    [Test]
    public void LoadLibrary_WhitespacePath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Z3.LoadLibrary("   "));
    }

    [Test]
    public void LoadLibrary_NonexistentPath_ThrowsFileNotFoundException()
    {
        var nonexistentPath = "/completely/nonexistent/path/to/library.so";
        Assert.Throws<FileNotFoundException>(() => Z3.LoadLibrary(nonexistentPath));
    }

    [Test]
    public void LoadLibraryAuto_DoesNotThrow()
    {
        // LoadLibraryAuto should either succeed or throw InvalidOperationException
        // if no Z3 library is found, but it shouldn't throw other exceptions
        try
        {
            Z3.LoadLibraryAuto();
            // If it succeeds, that's great - Z3 is available
        }
        catch (InvalidOperationException)
        {
            // This is expected if Z3 is not installed in standard locations
            Assert.Pass("Z3 library not found in standard locations, which is expected in test environment");
        }
    }

    [Test]
    public void LoadLibraryAuto_WhenAlreadyLoaded_DoesNotReload()
    {
        // First, try to load automatically
        try
        {
            Z3.LoadLibraryAuto();

            // If successful, calling it again should not reload (returns early)
            Assert.DoesNotThrow(Z3.LoadLibraryAuto);
        }
        catch (InvalidOperationException)
        {
            // Z3 not available in test environment - skip this test
            Assert.Ignore("Z3 library not available for testing library replacement behavior");
        }
    }
}
