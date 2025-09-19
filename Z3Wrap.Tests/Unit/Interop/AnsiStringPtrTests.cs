using Spaceorc.Z3Wrap.Interop;

namespace Z3Wrap.Tests.Unit.Interop;

[TestFixture]
public class AnsiStringPtrTests
{
    [Test]
    public void Constructor_ValidString_CreatesNonNullPointer()
    {
        using var stringPtr = new AnsiStringPtr("test");
        IntPtr pointer = stringPtr;

        Assert.That(pointer, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Constructor_EmptyString_CreatesNonNullPointer()
    {
        using var stringPtr = new AnsiStringPtr("");
        IntPtr pointer = stringPtr;

        Assert.That(pointer, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Constructor_NullString_DoesNotThrow()
    {
        // AnsiStringPtr constructor doesn't validate null input, it passes through to Marshal.StringToHGlobalAnsi
        // which handles null gracefully by returning IntPtr.Zero
        Assert.DoesNotThrow(() => new AnsiStringPtr(null!));
    }

    [Test]
    public void ImplicitConversion_ValidString_WorksCorrectly()
    {
        using var stringPtr = new AnsiStringPtr("test");
        IntPtr pointer = stringPtr; // Implicit conversion

        Assert.That(pointer, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Dispose_CalledMultipleTimes_DoesNotThrow()
    {
        var stringPtr = new AnsiStringPtr("test");

        Assert.DoesNotThrow(() => stringPtr.Dispose());
        Assert.DoesNotThrow(() => stringPtr.Dispose()); // Second dispose should not throw
    }

    [Test]
    public void UsingStatement_AutomaticallyDisposes()
    {
        AnsiStringPtr? stringPtr = null;

        Assert.DoesNotThrow(() =>
        {
            using (stringPtr = new AnsiStringPtr("test"))
            {
                IntPtr pointer = (IntPtr)stringPtr;
                Assert.That(pointer, Is.Not.EqualTo(IntPtr.Zero));
            }
            // stringPtr should be disposed here
        });
    }

    [Test]
    public void Constructor_UnicodeString_HandlesCorrectly()
    {
        // AnsiStringPtr should handle Unicode by converting to ANSI
        using var stringPtr = new AnsiStringPtr("tëst");
        IntPtr pointer = stringPtr;

        Assert.That(pointer, Is.Not.EqualTo(IntPtr.Zero));
    }
}
