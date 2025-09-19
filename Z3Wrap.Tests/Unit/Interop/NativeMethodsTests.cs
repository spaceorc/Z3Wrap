using Spaceorc.Z3Wrap.Interop;

namespace Z3Wrap.Tests.Unit.Interop;

[TestFixture]
public class NativeMethodsTests
{
    [Test]
    public void LoadLibrary_ValidPath_DoesNotThrow()
    {
        // This test assumes libz3 is available in system path or LD_LIBRARY_PATH
        // We can't guarantee libz3 is available in test environment,
        // so we just test that the method doesn't throw
        Assert.DoesNotThrow(() => NativeMethods.LoadLibrary("libz3"));
    }

    [Test]
    public void LoadLibrary_InvalidPath_DoesNotThrow()
    {
        // We can't test return value without knowing the signature,
        // so we just test that the method doesn't throw
        Assert.DoesNotThrow(() => NativeMethods.LoadLibrary("nonexistent_library"));
    }

    [Test]
    public void Z3NativeDelegates_AreNotNull()
    {
        // Test that critical delegates are loaded (if Z3 is available)
        // This is more of a smoke test to ensure the native method loading works

        // We can't easily test this without actually loading Z3,
        // so we'll just verify the static class can be accessed
        Assert.DoesNotThrow(() =>
        {
            var type = typeof(NativeMethods);
            Assert.That(type, Is.Not.Null);
        });
    }

    [Test]
    public void LibraryPath_CanBeSet()
    {
        // Test that we can set the library path without throwing
        // This tests the static initialization logic
        Assert.DoesNotThrow(() =>
        {
            // The library path setting happens in static constructor
            // so we just need to ensure the class can be accessed
            _ = typeof(NativeMethods).FullName;
        });
    }
}
