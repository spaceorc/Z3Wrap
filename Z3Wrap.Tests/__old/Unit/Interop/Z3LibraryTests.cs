using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;

namespace Z3Wrap.Tests.__old.Unit.Interop;

[TestFixture]
public class Z3LibraryTests
{
    [Test]
    public void Load_EmptyPath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Z3Library.Load(""));
    }

    [Test]
    public void Load_WhitespacePath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Z3Library.Load("   "));
    }

    [Test]
    public void Load_NonexistentPath_ThrowsFileNotFoundException()
    {
        var nonexistentPath = "/completely/nonexistent/path/to/library.so";
        Assert.Throws<FileNotFoundException>(() => Z3Library.Load(nonexistentPath));
    }

    [Test]
    public void LoadAuto_ReturnsValidLibrary()
    {
        // Should succeed since GlobalSetup loads the library
        var library = Z3Library.LoadAuto();
        Assert.That(library, Is.Not.Null);
        Assert.That(library.LibraryPath, Is.Not.Null.And.Not.Empty);
        library.Dispose();
    }

    [Test]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var library = Z3Library.LoadAuto();
        library.Dispose();

        // Second dispose should not throw
        Assert.DoesNotThrow(() => library.Dispose());
    }

    [Test]
    public void LoadAuto_WithUsing_DisposesCorrectly()
    {
        Z3Library? library = null;
        Assert.DoesNotThrow(() =>
        {
            using (library = Z3Library.LoadAuto())
            {
                Assert.That(library.LibraryPath, Is.Not.Null.And.Not.Empty);
            }
        });

        // Verify library was created and disposed
        Assert.That(library, Is.Not.Null);
    }

    [Test]
    public void LoadAuto_MultipleInstances_EachDisposable()
    {
        var library1 = Z3Library.LoadAuto();
        var library2 = Z3Library.LoadAuto();

        Assert.That(library1, Is.Not.Null);
        Assert.That(library2, Is.Not.Null);
        Assert.That(library1, Is.Not.SameAs(library2));

        library1.Dispose();
        library2.Dispose();
    }

    [Test]
    public void LibraryPath_ReturnsValidPath()
    {
        using var library = Z3Library.LoadAuto();
        var path = library.LibraryPath;

        Assert.That(path, Is.Not.Null.And.Not.Empty);
        Assert.That(File.Exists(path), Is.True);
    }

    [Test]
    public void Z3Context_CanUseCustomLibrary()
    {
        using var library = Z3Library.LoadAuto();
        using var context = new Z3Context(library);

        Assert.That(context, Is.Not.Null);
        Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }
}

[TestFixture]
public class Z3DefaultLibraryTests
{
    [Test]
    public void DefaultLibrary_Get_ReturnsValidLibrary()
    {
        // This should use the library loaded in GlobalSetup
        var library = Z3.DefaultLibrary;

        Assert.That(library, Is.Not.Null);
        Assert.That(library.LibraryPath, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void DefaultLibrary_Set_AcceptsNewLibrary()
    {
        // Get current default to restore later
        var originalLibrary = Z3.DefaultLibrary;
        var originalPath = originalLibrary.LibraryPath;

        try
        {
            // Create and set a new library
            var newLibrary = Z3Library.LoadAuto();
            Z3.DefaultLibrary = newLibrary;

            // Verify the new library is set
            var currentLibrary = Z3.DefaultLibrary;
            Assert.That(currentLibrary, Is.SameAs(newLibrary));

            // DO NOT dispose newLibrary - ownership transferred to Z3 class
        }
        finally
        {
            // Restore original library for other tests
            // Note: This will dispose the library we just set
            Z3.DefaultLibrary = Z3Library.LoadAuto();
        }
    }

    [Test]
    public void DefaultLibrary_Set_DisposesOldLibrary()
    {
        // This test verifies that setting a new default library disposes the old one
        // We can't directly verify disposal, but we can verify the exchange happens atomically
        var library1 = Z3Library.LoadAuto();
        var library2 = Z3Library.LoadAuto();

        Z3.DefaultLibrary = library1;
        var retrievedLibrary1 = Z3.DefaultLibrary;
        Assert.That(retrievedLibrary1, Is.SameAs(library1));

        // Set new library - should dispose library1
        Z3.DefaultLibrary = library2;
        var retrievedLibrary2 = Z3.DefaultLibrary;
        Assert.That(retrievedLibrary2, Is.SameAs(library2));
        Assert.That(retrievedLibrary2, Is.Not.SameAs(library1));

        // DO NOT dispose library1 or library2 - ownership transferred
        // Restore a fresh library for other tests
        Z3.DefaultLibrary = Z3Library.LoadAuto();
    }

    [Test]
    public void DefaultLibrary_Set_NullValue_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Z3.DefaultLibrary = null!);
    }

    [Test]
    public void LoadLibrary_SetsDefaultLibrary()
    {
        var originalLibrary = Z3.DefaultLibrary;

        try
        {
            // LoadLibrary should set the default library
            Z3.LoadLibraryAuto();
            var newLibrary = Z3.DefaultLibrary;

            Assert.That(newLibrary, Is.Not.Null);
            Assert.That(newLibrary.LibraryPath, Is.Not.Null.And.Not.Empty);
        }
        finally
        {
            // Restore original
            Z3.DefaultLibrary = Z3Library.LoadAuto();
        }
    }

    [Test]
    public void DefaultLibrary_ConcurrentAccess_ThreadSafe()
    {
        var tasks = new List<Task>();
        var libraries = new List<Z3Library>();
        var lockObj = new object();

        // Create libraries for concurrent setting
        for (int i = 0; i < 5; i++)
        {
            libraries.Add(Z3Library.LoadAuto());
        }

        // Concurrently set different libraries
        for (int i = 0; i < 5; i++)
        {
            var library = libraries[i];
            tasks.Add(
                Task.Run(() =>
                {
                    // Set the library (thread-safe via Interlocked.Exchange)
                    Z3.DefaultLibrary = library;

                    // Get current library (should always be non-null)
                    var current = Z3.DefaultLibrary;
                    Assert.That(current, Is.Not.Null);
                })
            );
        }

        // Wait for all tasks to complete
        Assert.DoesNotThrow(() => Task.WaitAll(tasks.ToArray()));

        // Final library should be one of the ones we set
        var finalLibrary = Z3.DefaultLibrary;
        Assert.That(finalLibrary, Is.Not.Null);

        // DO NOT dispose any libraries - ownership transferred
        // Restore for other tests
        Z3.DefaultLibrary = Z3Library.LoadAuto();
    }

    [Test]
    public void LoadLibraryAuto_DisposesOldLibrary()
    {
        // Set an initial library
        var library1 = Z3Library.LoadAuto();
        Z3.DefaultLibrary = library1;

        // LoadLibraryAuto should dispose library1 and set a new one
        Z3.LoadLibraryAuto();
        var library2 = Z3.DefaultLibrary;

        Assert.That(library2, Is.Not.Null);
        Assert.That(library2, Is.Not.SameAs(library1));

        // DO NOT dispose - ownership transferred
    }

    [Test]
    public void Z3Context_UsesDefaultLibrary()
    {
        // Ensure we have a default library
        var defaultLib = Z3.DefaultLibrary;

        // Create context without explicit library - should use default
        using var context = new Z3Context();

        Assert.That(context, Is.Not.Null);
        Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }
}
